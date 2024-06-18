using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Services;

public interface ILeaveRequestService
{
    Task<List<GetLeaveRequestDto>> GetAllLeaveRequestsAsync(string searchTerm = null);
    Task<GetLeaveRequestDto> GetLeaveRequestByIdAsync(int id);
    Task<GetLeaveRequestDto> AddLeaveRequestAsync(AddLeaveRequestDto leaveRequest);
    Task<GetLeaveRequestDto> UpdateLeaveRequestInfoAsync(int id, UpdateLeaveRequestDto leaveRequest);
    Task<GetLeaveRequestDto> UpdateLeaveRequestStatusAsync(int id, int statusId);
    Task<List<AbsenceReason>> GetAllAbsenceReasonsAsync();
    Task<List<RequestType>> GetAllTypesAsync();
    Task<List<LeaveRequestStatus>> GetAllStatusesAsync();
}
