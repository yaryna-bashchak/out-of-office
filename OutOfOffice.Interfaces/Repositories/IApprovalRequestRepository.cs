using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface IApprovalRequestRepository
{
    Task<List<ApprovalRequest>> GetAllApprovalRequestsAsync(string searchTerm = null);
    Task<ApprovalRequest> GetApprovalRequestByIdAsync(int id);
    Task<ApprovalRequest> AddApprovalRequestAsync(ApprovalRequest approvalRequest);
    Task<ApprovalRequest> UpdateApprovalRequestAsync(ApprovalRequest approvalRequest);
    Task<List<ApprovalRequestStatus>> GetAllStatusesAsync();
    Task<List<ApprovalRequest>> GetAllApprovalRequestsByLeaveRequestIdAsync(int leaveRequestId);
}
