using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Services;

public class LeaveRequestService : ILeaveRequestService
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IApprovalRequestRepository _approvalRequestRepository;

    public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, IEmployeeRepository employeeRepository, IApprovalRequestRepository approvalRequestRepository)
    {
        _approvalRequestRepository = approvalRequestRepository;
        _leaveRequestRepository = leaveRequestRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<List<GetLeaveRequestDto>> GetAllLeaveRequestsAsync()
    {
        var leaveRequests = await _leaveRequestRepository.GetAllLeaveRequestsAsync();
        return leaveRequests.Select(lr => MapToLeaveRequestDto(lr)).ToList();
    }

    public async Task<GetLeaveRequestDto> GetLeaveRequestByIdAsync(int id)
    {
        var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(id);
        return MapToLeaveRequestDto(leaveRequest);
    }

    public async Task<GetLeaveRequestDto> AddLeaveRequestAsync(AddLeaveRequestDto leaveRequestDto)
    {
        // check if the employee's OutOfOffice balance is sufficient to receive this leave request
        decimal workingDays = await CalculateWorkingDaysAsync(leaveRequestDto.RequestTypeId, leaveRequestDto.Hours, leaveRequestDto.StartDate, leaveRequestDto.EndDate);

        var employee = await _employeeRepository.GetEmployeeByIdAsync(leaveRequestDto.EmployeeId);
        if (employee.OutOfOfficeBalance < workingDays)
        {
            throw new InvalidOperationException($"You cannot create this request because your current OutOfOfficeBalance ({employee.OutOfOfficeBalance} days) is less than the specified number of days ({workingDays} days).");
        }

        // map LeaveRequestDto to LeaveRequest
        var mappedLeaveRequest = MapToLeaveRequest(leaveRequestDto);

        // set status to "New"
        var statuses = await _leaveRequestRepository.GetAllStatusesAsync();
        var newStatusId = statuses.Find(s => s.Name == "New").Id;
        mappedLeaveRequest.StatusId = newStatusId;

        // create LeaveRequest
        var createdLeaveRequest = await _leaveRequestRepository.AddLeaveRequestAsync(mappedLeaveRequest);
        return MapToLeaveRequestDto(createdLeaveRequest);
    }

    public async Task<GetLeaveRequestDto> UpdateLeaveRequestInfoAsync(int id, UpdateLeaveRequestDto leaveRequestDto)
    {
        var prevLeaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(id);
        var leaveRequestStatuses = await _leaveRequestRepository.GetAllStatusesAsync();
        var status = GetStatusName(leaveRequestStatuses, prevLeaveRequest.StatusId);

        ValidateRequestStatus(id, status, "New");

        decimal workingDays = await CalculateWorkingDaysAsync(leaveRequestDto.RequestTypeId, leaveRequestDto.Hours, leaveRequestDto.StartDate, leaveRequestDto.EndDate);

        var employee = await _employeeRepository.GetEmployeeByIdAsync(leaveRequestDto.EmployeeId);
        ValidateOutOfOfficeBalance(employee.OutOfOfficeBalance, workingDays);

        var mappedLeaveRequest = MapToLeaveRequest(id, leaveRequestDto);
        var updatedLeaveRequest = await _leaveRequestRepository.UpdateLeaveRequestAsync(mappedLeaveRequest);

        return MapToLeaveRequestDto(updatedLeaveRequest);
    }

    public async Task<GetLeaveRequestDto> UpdateLeaveRequestStatusAsync(int requestId, int statusId)
    {
        var prevLeaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(requestId);
        var leaveRequestStatuses = await _leaveRequestRepository.GetAllStatusesAsync();
        var prevStatus = GetStatusName(leaveRequestStatuses, prevLeaveRequest.StatusId);
        var curStatus = GetStatusName(leaveRequestStatuses, statusId);

        ValidateRequestStatus(requestId, prevStatus, "New", "Submitted");

        var updatedLeaveRequest = await UpdateLeaveRequestStatus(requestId, prevLeaveRequest, statusId);

        await HandleApprovalRequestsAsync(requestId, prevStatus, curStatus, prevLeaveRequest.EmployeeId);

        return MapToLeaveRequestDto(updatedLeaveRequest);
    }

    // private methods
    private async Task<decimal> CalculateWorkingDaysAsync(int requestTypeId, int? hours, DateTime startDate, DateTime endDate)
    {
        var requestTypes = await _leaveRequestRepository.GetAllRequestTypesAsync();
        var partialDayTypeId = requestTypes.Find(rt => rt.Name == "Partial day").Id;

        if (requestTypeId == partialDayTypeId)
        {
            if (hours.HasValue)
            {
                return hours.Value / 8m;
            }
            else
            {
                throw new InvalidOperationException("If request type is partial day, then hours are required.");
            }
        }
        else
        {
            return (endDate.Date - startDate.Date).Days + 1;
        }
    }
    private string GetStatusName(List<LeaveRequestStatus> statuses, int statusId)
    {
        return statuses.Find(s => s.Id == statusId).Name;
    }

    private void ValidateRequestStatus(int id, string currentStatus, params string[] validStatuses)
    {
        if (!validStatuses.Contains(currentStatus))
        {
            throw new InvalidOperationException($"Leave Request with ID {id} is already {currentStatus.ToLower()}. It can not be edited.");
        }
    }

    private void ValidateOutOfOfficeBalance(decimal balance, decimal requiredDays)
    {
        if (balance < requiredDays)
        {
            throw new InvalidOperationException($"Current OutOfOfficeBalance ({balance} days) is less than the specified number of days ({requiredDays} days).");
        }
    }

    private async Task<LeaveRequest> UpdateLeaveRequestStatus(int id, LeaveRequest prevLeaveRequest, int statusId)
    {
        var leaveRequest = new LeaveRequest
        {
            Id = id,
            StartDate = prevLeaveRequest.StartDate,
            EndDate = prevLeaveRequest.EndDate,
            Hours = prevLeaveRequest.Hours,
            Comment = prevLeaveRequest.Comment,
            EmployeeId = prevLeaveRequest.EmployeeId,
            AbsenceReasonId = prevLeaveRequest.AbsenceReasonId,
            RequestTypeId = prevLeaveRequest.RequestTypeId,
            StatusId = statusId,
        };

        return await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);
    }

    private async Task HandleApprovalRequestsAsync(int id, string prevStatus, string curStatus, int employeeId)
    {
        var statuses = await _approvalRequestRepository.GetAllStatusesAsync();

        if (prevStatus == "New" && curStatus == "Submitted")
        {
            var newStatusId = statuses.Find(s => s.Name == "New").Id;
            await CreateApprovalRequestsAsync(id, newStatusId, employeeId);
        }
        else if (prevStatus == "Submitted" && curStatus == "Cancelled")
        {
            var newApprovalRequestStatusId = statuses.Find(s => s.Name == "Cancelled").Id;
            await UpdateApprovalRequestsAsync(id, newApprovalRequestStatusId);
        }
    }

    private async Task CreateApprovalRequestsAsync(int leaveRequestId, int newStatusId, int employeeId)
    {
        var approverIds = await _employeeRepository.GetAllProjectManagerIdsOfEmployeeAsync(employeeId);
        var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);

        if (employee.PeoplePartnerId != null)
        {
            approverIds.Add(employee.PeoplePartnerId.Value);
        }

        foreach (var approverId in approverIds)
        {
            await _approvalRequestRepository.AddApprovalRequestAsync(new ApprovalRequest
            {
                LeaveRequestId = leaveRequestId,
                StatusId = newStatusId,
                ApproverId = approverId
            });
        }
    }

    private async Task UpdateApprovalRequestsAsync(int leaveRequestId, int newStatusId)
    {
        var approvalRequests = await _approvalRequestRepository.GetAllApprovalRequestsByLeaveRequestIdAsync(leaveRequestId);

        foreach (var approvalRequest in approvalRequests)
        {
            var updatedApprovalRequest = new ApprovalRequest
            {
                Id = approvalRequest.Id,
                Comment = approvalRequest.Comment,
                ApproverId = approvalRequest.ApproverId,
                LeaveRequestId = approvalRequest.LeaveRequestId,
                StatusId = newStatusId
            };

            await _approvalRequestRepository.UpdateApprovalRequestAsync(updatedApprovalRequest);
        }
    }

    public static GetLeaveRequestDto MapToLeaveRequestDto(LeaveRequest leaveRequest)
    {
        return new GetLeaveRequestDto
        {
            Id = leaveRequest.Id,
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            Hours = leaveRequest.Hours,
            Comment = leaveRequest.Comment,
            Employee = new GetEmployeeDto { Id = leaveRequest.EmployeeId, FullName = leaveRequest.Employee.FullName },
            AbsenceReason = leaveRequest.AbsenceReason,
            RequestType = leaveRequest.RequestType,
            Status = leaveRequest.Status,
        };
    }

    private static LeaveRequest MapToLeaveRequest(AddLeaveRequestDto leaveRequestDto)
    {
        return new LeaveRequest
        {
            StartDate = leaveRequestDto.StartDate,
            EndDate = leaveRequestDto.EndDate,
            Hours = leaveRequestDto.Hours,
            Comment = leaveRequestDto.Comment,
            EmployeeId = leaveRequestDto.EmployeeId,
            AbsenceReasonId = leaveRequestDto.AbsenceReasonId,
            RequestTypeId = leaveRequestDto.RequestTypeId,
        };
    }

    private static LeaveRequest MapToLeaveRequest(int id, UpdateLeaveRequestDto leaveRequestDto)
    {
        return new LeaveRequest
        {
            Id = id,
            StartDate = leaveRequestDto.StartDate,
            EndDate = leaveRequestDto.EndDate,
            Hours = leaveRequestDto.Hours,
            Comment = leaveRequestDto.Comment,
            EmployeeId = leaveRequestDto.EmployeeId,
            AbsenceReasonId = leaveRequestDto.AbsenceReasonId,
            RequestTypeId = leaveRequestDto.RequestTypeId,
        };
    }
}

