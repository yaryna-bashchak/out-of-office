using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee> GetByIdAsync(int id);
    Task<Employee> AddAsync(Employee newObject);
    Task<Employee> UpdateAsync(Employee updatedObject);
    Task<bool> DeleteAsync(int id);
}
