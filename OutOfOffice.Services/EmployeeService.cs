using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(e => MapToEmployeeDto(e)).ToList();
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null) return null;
        return MapToEmployeeDto(employee);
    }

    private EmployeeDto MapToEmployeeDto(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            OutOfOfficeBalance = employee.OutOfOfficeBalance,
            Photo = employee.Photo,
            PeoplePartner = employee.PeoplePartner != null ? MapToPeoplePartnerDto(employee.PeoplePartner) : null,
            Position = employee.Position,
            Status = employee.Status,
            Subdivision = employee.Subdivision
        };
    }

    private PeoplePartnerDto MapToPeoplePartnerDto(Employee employee)
    {
        return new PeoplePartnerDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
        };
    }
}

