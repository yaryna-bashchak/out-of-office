using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllEmployeesAsync(string searchTerm = null);
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task<Employee> AddEmployeeAsync(Employee employee);
    Task<Employee> UpdateEmployeeAsync(Employee employee);
    Task<List<int>> GetAllProjectManagerIdsOfEmployeeAsync(int employeeId);
    Task<List<ProjectEmployee>> GetProjectEmployeesByProjectIdAsync(int projectId);
    Task<List<ProjectEmployee>> GetProjectEmployeesByEmployeeIdAsync(int employeeId);
    Task<List<Subdivision>> GetAllSubdivisionsAsync();
    Task<List<Position>> GetAllPositionsAsync();
    Task<List<EmployeeStatus>> GetAllStatusesAsync();
    Task<List<Employee>> GetAllEmployeesByPositionAsync(string positionName);
}
