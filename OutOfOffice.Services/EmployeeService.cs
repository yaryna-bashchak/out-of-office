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
        var employees = await _employeeRepository.GetAllEmployeesAsync();
        return employees.Select(e => MapToEmployeeDto(e)).ToList();
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (employee == null) return null;
        return MapToEmployeeDto(employee);
    }

    public async Task<EmployeeDto> AddEmployeeAsync(EmployeeDto newEmployeeDto)
    {
        var employee = MapToEmployee(newEmployeeDto);
        var createdEmployee = await _employeeRepository.AddEmployeeAsync(employee);
        if (createdEmployee == null) return null;
        return MapToEmployeeDto(createdEmployee);
    }

    public async Task<EmployeeDto> UpdateEmployeeAsync(EmployeeDto updatedEmployeeDto)
    {
        var employee = MapToEmployee(updatedEmployeeDto);
        var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(employee);
        if (updatedEmployee == null) return null;
        return MapToEmployeeDto(updatedEmployee);
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
            Subdivision = employee.Subdivision,
        };
    }

    private Employee MapToEmployee(EmployeeDto employeeDto)
    {
        return new Employee
        {
            Id = employeeDto.Id,
            FullName = employeeDto.FullName,
            OutOfOfficeBalance = employeeDto.OutOfOfficeBalance,
            Photo = employeeDto.Photo,
            PeoplePartnerId = employeeDto.PeoplePartner?.Id,
            PositionId = employeeDto.Position.Id,
            StatusId = employeeDto.Status.Id,
            SubdivisionId = employeeDto.Subdivision.Id,
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

