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
        decimal workingDays;
        var requestType = await _leaveRequestRepository.GetAllRequestTypesAsync();
        var partialDayTypeId = requestType.Find(rt => rt.Name == "Partial day").Id;

        if (leaveRequestDto.RequestTypeId == partialDayTypeId && leaveRequestDto.Hours.HasValue)
        {
            workingDays = leaveRequestDto.Hours.Value / 8m;
        }
        else
        {
            workingDays = leaveRequestDto.EndDate.DayNumber - leaveRequestDto.StartDate.DayNumber;
        }

        var employee = await _employeeRepository.GetEmployeeByIdAsync(leaveRequestDto.EmployeeId);
        if (employee.OutOfOfficeBalance < workingDays)
        {
            throw new InvalidOperationException($"You cannot create this request because your current OutOfOfficeBalance ({employee.OutOfOfficeBalance} days) is less than the specified number of days ({workingDays} days).");
        }

        var mappedLeaveRequest = MapToLeaveRequest(leaveRequestDto);
        var createdLeaveRequest = await _leaveRequestRepository.AddLeaveRequestAsync(mappedLeaveRequest);
        return MapToLeaveRequestDto(createdLeaveRequest);
    }

    public async Task<GetLeaveRequestDto> UpdateLeaveRequestAsync(int id, UpdateLeaveRequestDto leaveRequest)
    {
        // add creating and updating approval requests
        var mappedLeaveRequest = MapToLeaveRequest(id, leaveRequest);
        var updatedLeaveRequest = await _leaveRequestRepository.UpdateLeaveRequestAsync(mappedLeaveRequest);
        return MapToLeaveRequestDto(updatedLeaveRequest);
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
            StatusId = leaveRequestDto.StatusId,
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

