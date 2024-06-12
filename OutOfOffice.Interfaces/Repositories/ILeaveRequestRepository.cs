using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface ILeaveRequestRepository
{
    Task<List<LeaveRequest>> GetAll();
    Task<LeaveRequest> GetById(int id);
    Task<LeaveRequest> Add(LeaveRequest newObject);
    Task<LeaveRequest> Update(LeaveRequest updatedObject);
    Task<bool> Delete(int id);
}
