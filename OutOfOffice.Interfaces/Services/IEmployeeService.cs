using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Services;

public interface IEmployeeService
{
    Task<List<GetEmployeeDto>> GetAllEmployeesAsync(string searchTerm = null);
    Task<GetEmployeeDto> GetEmployeeByIdAsync(int id);
    Task<GetEmployeeDto> AddEmployeeAsync(AddEmployeeDto newEmployee);
    Task<GetEmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto updatedEmployee);
    Task<List<Subdivision>> GetAllSubdivisionsAsync();
    Task<List<Position>> GetAllPositionsAsync();
    Task<List<EmployeeStatus>> GetAllStatusesAsync();
    Task<List<GetEmployeeDto>> GetAllEmployeesByPositionAsync(string positionName);
}
