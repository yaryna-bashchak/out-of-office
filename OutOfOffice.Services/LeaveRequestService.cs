using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Services;

public class LeaveRequestService : ILeaveRequestService
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, IEmployeeRepository employeeRepository)
    {
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

    public async Task<GetLeaveRequestDto> UpdateLeaveRequestAsync(int id, UpdateLeaveRequestDto leaveRequestDto)
    {
        // check if the employee's OutOfOffice balance is sufficient to receive this leave request
        decimal workingDays = await CalculateWorkingDaysAsync(leaveRequestDto.RequestTypeId, leaveRequestDto.Hours, leaveRequestDto.StartDate, leaveRequestDto.EndDate);

        var employee = await _employeeRepository.GetEmployeeByIdAsync(leaveRequestDto.EmployeeId);
        if (employee.OutOfOfficeBalance < workingDays)
        {
            throw new InvalidOperationException($"You cannot create this request because your current OutOfOfficeBalance ({employee.OutOfOfficeBalance} days) is less than the specified number of days ({workingDays} days).");
        }

        // add creating and updating approval requests
        var mappedLeaveRequest = MapToLeaveRequest(id, leaveRequestDto);
        var updatedLeaveRequest = await _leaveRequestRepository.UpdateLeaveRequestAsync(mappedLeaveRequest);
        return MapToLeaveRequestDto(updatedLeaveRequest);
    }

    private async Task<decimal> CalculateWorkingDaysAsync(int requestTypeId, int? hours, DateOnly startDate, DateOnly endDate)
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
            return endDate.DayNumber - startDate.DayNumber + 1;
        }
    }

    private static GetLeaveRequestDto MapToLeaveRequestDto(LeaveRequest leaveRequest)
    {
        return new GetLeaveRequestDto
        {
            Id = leaveRequest.Id,
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            Hours = leaveRequest.Hours,
            Comment = leaveRequest.Comment,
            Employee = new GetEmployeeDto { Id = leaveRequest.Employee.Id, FullName = leaveRequest.Employee.FullName },
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
            StartDate = leaveRequestDto.StartDate,
            EndDate = leaveRequestDto.EndDate,
            Hours = leaveRequestDto.Hours,
            Comment = leaveRequestDto.Comment,
            EmployeeId = leaveRequestDto.EmployeeId,
            AbsenceReasonId = leaveRequestDto.AbsenceReasonId,
            RequestTypeId = leaveRequestDto.RequestTypeId,
            StatusId = leaveRequestDto.StatusId,
        };
    }
}

