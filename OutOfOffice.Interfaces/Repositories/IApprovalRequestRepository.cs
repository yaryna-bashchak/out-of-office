using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface IApprovalRequestRepository
{
    Task<List<ApprovalRequest>> GetAll();
    Task<ApprovalRequest> GetById(int id);
    Task<ApprovalRequest> Add(ApprovalRequest newObject);
    Task<ApprovalRequest> Update(ApprovalRequest updatedObject);
    Task<bool> Delete(int id);
}
