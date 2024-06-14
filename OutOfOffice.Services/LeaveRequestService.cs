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
        var status = leaveRequestStatuses.Find(s => s.Id == prevLeaveRequest.StatusId).Name;

        // request info can only be updated if it is "New"
        if (status != "New")
        {
            throw new InvalidOperationException($"Leave Request with ID {id} is already {status.ToLower()}. It can not be edited.");
        }

        // check if the employee's OutOfOffice balance is sufficient to receive this leave request
        decimal workingDays = await CalculateWorkingDaysAsync(leaveRequestDto.RequestTypeId, leaveRequestDto.Hours, leaveRequestDto.StartDate, leaveRequestDto.EndDate);

        var employee = await _employeeRepository.GetEmployeeByIdAsync(leaveRequestDto.EmployeeId);
        if (employee.OutOfOfficeBalance < workingDays)
        {
            throw new InvalidOperationException($"Current OutOfOfficeBalance ({employee.OutOfOfficeBalance} days) is less than the specified number of days ({workingDays} days).");
        }

        var mappedLeaveRequest = MapToLeaveRequest(id, leaveRequestDto);
        var updatedLeaveRequest = await _leaveRequestRepository.UpdateLeaveRequestAsync(mappedLeaveRequest);
        return MapToLeaveRequestDto(updatedLeaveRequest);
    }

    public async Task<GetLeaveRequestDto> UpdateLeaveRequestStatusAsync(int id, int statusId)
    {
        var prevLeaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(id);
        var leaveRequestStatuses = await _leaveRequestRepository.GetAllStatusesAsync();
        var prevStatus = leaveRequestStatuses.Find(s => s.Id == prevLeaveRequest.Status.Id).Name;
        var employee = await _employeeRepository.GetEmployeeByIdAsync(prevLeaveRequest.EmployeeId);

        // request status can only be updated if it is "New" or "Submitted" 
        if (prevStatus != "New" && prevStatus != "Submitted")
        {
            throw new InvalidOperationException($"Leave Request with ID {id} is already {prevStatus.ToLower()}. Its status can not be edited.");
        }

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
        var updatedLeaveRequest = await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);

        // create or update appropriate approval requests
        var approvalRequestStatuses = await _approvalRequestRepository.GetAllStatusesAsync();
        var curStatus = leaveRequestStatuses.Find(s => s.Id == statusId).Name;

        if (prevStatus == "New" && curStatus == "Submitted")
        {
            var newApprovalRequestStatusId = approvalRequestStatuses.Find(s => s.Name == "New").Id;
            var approverIds = await _employeeRepository.GetAllProjectManagerIdsOfEmployeeAsync(prevLeaveRequest.EmployeeId);
            if (employee.PeoplePartner != null) approverIds.Add(employee.PeoplePartner.Id);

            foreach (var approverId in approverIds)
            {
                await _approvalRequestRepository.AddApprovalRequestAsync(new ApprovalRequest { LeaveRequestId = id, StatusId = newApprovalRequestStatusId, ApproverId = approverId });
            }
        }
        else if (prevStatus == "Submitted" && curStatus == "Cancelled")
        {
            var newApprovalRequestStatusId = approvalRequestStatuses.Find(s => s.Name == "Cancelled").Id;
            var approvalRequests = await _approvalRequestRepository.GetAllApprovalRequestsByLeaveRequestIdAsync(id);

            foreach (var approvalRequest in approvalRequests)
            {
                var updatedApprovalRequest = new ApprovalRequest
                {
                    Id = approvalRequest.Id,
                    Comment = approvalRequest.Comment,
                    ApproverId = approvalRequest.ApproverId,
                    LeaveRequestId = approvalRequest.LeaveRequestId,
                    StatusId = newApprovalRequestStatusId
                };

                await _approvalRequestRepository.UpdateApprovalRequestAsync(updatedApprovalRequest);
            }
        }

        return MapToLeaveRequestDto(updatedLeaveRequest);
    }

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

