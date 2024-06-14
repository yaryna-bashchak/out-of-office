using OutOfOffice.Contracts.DTOs;

namespace OutOfOffice.Interfaces.Services;

public interface IApprovalRequestService
{
    Task<List<GetApprovalRequestDto>> GetAllApprovalRequestsAsync();
    Task<GetApprovalRequestDto> GetApprovalRequestByIdAsync(int id);
    Task<GetApprovalRequestDto> UpdateApprovalRequestAsync(int id, UpdateApprovalRequestDto approvalRequest);
}
