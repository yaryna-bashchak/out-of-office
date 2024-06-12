using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAll();
    Task<Employee> GetById(int id);
    Task<Employee> Add(Employee newObject);
    Task<Employee> Update(Employee updatedObject);
    Task<bool> Delete(int id);
}
