using OutOfOffice.Contracts.DTOs;

namespace OutOfOffice.Interfaces.Services;

public interface ILeaveRequestService
{
    Task<List<GetLeaveRequestDto>> GetAllLeaveRequestsAsync();
    Task<GetLeaveRequestDto> GetLeaveRequestByIdAsync(int id);
    Task<GetLeaveRequestDto> AddLeaveRequestAsync(AddLeaveRequestDto leaveRequest);
    Task<GetLeaveRequestDto> UpdateLeaveRequestInfoAsync(int id, UpdateLeaveRequestDto leaveRequest);
    Task<GetLeaveRequestDto> UpdateLeaveRequestStatusAsync(int id, int statusId);
}
