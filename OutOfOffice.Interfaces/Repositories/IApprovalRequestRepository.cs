using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface IApprovalRequestRepository
{
    Task<List<ApprovalRequest>> GetAllAsync();
    Task<ApprovalRequest> GetByIdAsync(int id);
    Task<ApprovalRequest> AddAsync(ApprovalRequest newObject);
    Task<ApprovalRequest> UpdateAsync(ApprovalRequest updatedObject);
    Task<bool> DeleteAsync(int id);
}
