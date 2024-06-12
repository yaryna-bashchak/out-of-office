using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface ILeaveRequestRepository
{
    Task<List<LeaveRequest>> GetAllAsync();
    Task<LeaveRequest> GetByIdAsync(int id);
    Task<LeaveRequest> AddAsync(LeaveRequest newObject);
    Task<LeaveRequest> UpdateAsync(LeaveRequest updatedObject);
    Task<bool> DeleteAsync(int id);
}
