using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Services;

public class ApprovalRequestService : IApprovalRequestService
{
    private readonly IApprovalRequestRepository _approvalRequestRepository;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public ApprovalRequestService(IApprovalRequestRepository approvalRequestRepository, ILeaveRequestRepository leaveRequestRepository, IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
        _leaveRequestRepository = leaveRequestRepository;
        _approvalRequestRepository = approvalRequestRepository;

    }
    public async Task<List<GetApprovalRequestDto>> GetAllApprovalRequestsAsync()
    {
        var approvalRequests = await _approvalRequestRepository.GetAllApprovalRequestsAsync();
        return approvalRequests.Select(ar => MapToApprovalRequestDto(ar)).ToList();
    }

    public async Task<GetApprovalRequestDto> GetApprovalRequestByIdAsync(int id)
    {
        var approvalRequest = await _approvalRequestRepository.GetApprovalRequestByIdAsync(id);
        return MapToApprovalRequestDto(approvalRequest);
    }

    public async Task<GetApprovalRequestDto> UpdateApprovalRequestAsync(int requestId, UpdateApprovalRequestDto approvalRequest)
    {
        var prevRequest = await _approvalRequestRepository.GetApprovalRequestByIdAsync(requestId);

        ValidateRequestStatus(requestId, prevRequest.Status.Name, "New");

        var approvalStatuses = await _approvalRequestRepository.GetAllStatusesAsync();
        var prevStatus = prevRequest.Status.Name;
        var curStatus = approvalStatuses.Find(s => s.Id == approvalRequest.StatusId).Name;
        await HandleLeaveRequestAsync(prevRequest, prevStatus, curStatus);

        var mappedApprovalRequest = MapToApprovalRequest(approvalRequest, prevRequest);
        var updatedApprovalRequest = await _approvalRequestRepository.UpdateApprovalRequestAsync(mappedApprovalRequest);
        return MapToApprovalRequestDto(updatedApprovalRequest);
    }

    // private methods
    private static void ValidateRequestStatus(int id, string currentStatus, params string[] validStatuses)
    {
        if (!validStatuses.Contains(currentStatus))
        {
            throw new InvalidOperationException($"Approval Request with ID {id} is already {currentStatus.ToLower()}. It can not be edited.");
        }
    }

    private async Task HandleLeaveRequestAsync(ApprovalRequest approvalRequest, string prevStatus, string curStatus)
    {
        var prevRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(approvalRequest.LeaveRequestId);
        var leaveStatuses = await _leaveRequestRepository.GetAllStatusesAsync();

        if (prevStatus == "New" && curStatus == "Approved")
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(approvalRequest.LeaveRequest.EmployeeId);
            var requestTypes = await _leaveRequestRepository.GetAllRequestTypesAsync();
            decimal workingDays = LeaveRequestService.CalculateWorkingDaysAsync(prevRequest.RequestTypeId, prevRequest.Hours, prevRequest.StartDate, prevRequest.EndDate, requestTypes);

            LeaveRequestService.ValidateOutOfOfficeBalance(employee.OutOfOfficeBalance, workingDays);

            var approvalRequests = await _approvalRequestRepository.GetAllApprovalRequestsByLeaveRequestIdAsync(approvalRequest.LeaveRequestId);

            if (approvalRequests.All(ar => ar.Id == approvalRequest.Id || ar.Status.Name == "Approved"))
            {
                var newStatusId = leaveStatuses.Find(s => s.Name == "Approved").Id;
                var leaveRequest = LeaveRequestService.MapLeaveRequestStatus(approvalRequest.LeaveRequestId, newStatusId, prevRequest);
                await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);
                await _employeeRepository.UpdateEmployeeAsync(MapEmployeeBalance(employee, employee.OutOfOfficeBalance - workingDays));
            }
        }
        else if (prevStatus == "New" && curStatus == "Rejected")
        {
            var newStatusId = leaveStatuses.Find(s => s.Name == "Rejected").Id;
            var leaveRequest = LeaveRequestService.MapLeaveRequestStatus(approvalRequest.LeaveRequestId, newStatusId, prevRequest);
            var updatedLeaveRequest = await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);
        }
    }

    private static Employee MapEmployeeBalance(Employee employee, decimal balance)
    {
        return new Employee
        {
            Id = employee.Id,
            FullName = employee.FullName,
            OutOfOfficeBalance = balance,
            // Photo = employee.Photo,
            PeoplePartnerId = employee.PeoplePartnerId,
            PositionId = employee.PositionId,
            StatusId = employee.StatusId,
            SubdivisionId = employee.SubdivisionId,
        };
    }

    private static GetApprovalRequestDto MapToApprovalRequestDto(ApprovalRequest approvalRequest)
    {
        return new GetApprovalRequestDto
        {
            Id = approvalRequest.Id,
            Comment = approvalRequest.Comment,
            Approver = new GetEmployeeDto { Id = approvalRequest.Approver.Id, FullName = approvalRequest.Approver.FullName },
            LeaveRequest = LeaveRequestService.MapToLeaveRequestDto(approvalRequest.LeaveRequest),
            Status = approvalRequest.Status,
        };
    }

    private static ApprovalRequest MapToApprovalRequest(UpdateApprovalRequestDto approvalRequest, ApprovalRequest prevApprovalRequest)
    {
        return new ApprovalRequest
        {
            Id = prevApprovalRequest.Id,
            Comment = approvalRequest.Comment,
            ApproverId = prevApprovalRequest.ApproverId,
            LeaveRequestId = prevApprovalRequest.LeaveRequestId,
            StatusId = approvalRequest.StatusId,
        };
    }
}
