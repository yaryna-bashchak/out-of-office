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

    public async Task<List<GetEmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllEmployeesAsync();
        return employees.Select(e => MapToEmployeeDto(e)).ToList();
    }

    public async Task<GetEmployeeDto> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (employee == null) return null;
        return MapToEmployeeDto(employee);
    }

    public async Task<GetEmployeeDto> AddEmployeeAsync(AddEmployeeDto newEmployeeDto)
    {
        var employee = MapToEmployee(newEmployeeDto);
        var createdEmployee = await _employeeRepository.AddEmployeeAsync(employee);
        if (createdEmployee == null) return null;
        return MapToEmployeeDto(createdEmployee);
    }

    public async Task<GetEmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto updatedEmployeeDto)
    {
        var employee = MapToEmployee(id, updatedEmployeeDto);
        var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(employee);
        if (updatedEmployee == null) return null;
        return MapToEmployeeDto(updatedEmployee);
    }

    private static GetEmployeeDto MapToEmployeeDto(Employee employee)
    {
        return new GetEmployeeDto
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

    private static Employee MapToEmployee(AddEmployeeDto employeeDto)
    {
        return new Employee
        {
            FullName = employeeDto.FullName,
            OutOfOfficeBalance = employeeDto.OutOfOfficeBalance,
            // Photo = employeeDto.Photo,
            PeoplePartnerId = employeeDto.PeoplePartnerId,
            PositionId = employeeDto.PositionId,
            StatusId = employeeDto.StatusId,
            SubdivisionId = employeeDto.SubdivisionId,
        };
    }

    private static Employee MapToEmployee(int id, UpdateEmployeeDto employeeDto)
    {
        return new Employee
        {
            Id = id,
            FullName = employeeDto.FullName,
            OutOfOfficeBalance = employeeDto.OutOfOfficeBalance,
            // Photo = employeeDto.Photo,
            PeoplePartnerId = employeeDto.PeoplePartnerId,
            PositionId = employeeDto.PositionId,
            StatusId = employeeDto.StatusId,
            SubdivisionId = employeeDto.SubdivisionId,
        };
    }

    private static PeoplePartnerDto MapToPeoplePartnerDto(Employee employee)
    {
        return new PeoplePartnerDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
        };
    }
}

