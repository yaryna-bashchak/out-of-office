using OutOfOffice.Contracts.DTOs;

namespace OutOfOffice.Interfaces.Services;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto> GetEmployeeByIdAsync(int id);
}
