using OutOfOffice.Contracts.DTOs;

namespace OutOfOffice.Interfaces.Services;

public interface IEmployeeService
{
    Task<List<GetEmployeeDto>> GetAllEmployeesAsync();
    Task<GetEmployeeDto> GetEmployeeByIdAsync(int id);
    Task<GetEmployeeDto> AddEmployeeAsync(AddEmployeeDto newEmployee);
    Task<GetEmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto updatedEmployee);
}
