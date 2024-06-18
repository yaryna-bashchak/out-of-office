using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Services;

public interface IApprovalRequestService
{
    Task<List<GetApprovalRequestDto>> GetAllApprovalRequestsAsync(string searchTerm = null);
    Task<GetApprovalRequestDto> GetApprovalRequestByIdAsync(int id);
    Task<GetApprovalRequestDto> UpdateApprovalRequestAsync(int id, UpdateApprovalRequestDto approvalRequest);
    Task<List<ApprovalRequestStatus>> GetAllStatusesAsync();
}
