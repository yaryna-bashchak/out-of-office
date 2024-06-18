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

    public async Task<List<GetEmployeeDto>> GetAllEmployeesAsync(string searchTerm = null)
    {
        var employees = await _employeeRepository.GetAllEmployeesAsync(searchTerm);
        var employeeDtos = new List<GetEmployeeDto>();

        foreach (var employee in employees)
        {
            var employeeDto = CustomMapper.MapToEmployeeDto(employee);
            var employeeProjects = await _employeeRepository.GetProjectEmployeesByEmployeeIdAsync(employee.Id);
            foreach (var employeeProject in employeeProjects)
            {
                employeeDto.ProjectEmployees.Add(CustomMapper.MapToProjectEmployeeDto(employeeProject));
            }

            employeeDtos.Add(employeeDto);
        }
        return employeeDtos;
    }

    public async Task<GetEmployeeDto> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        var employeeDto = CustomMapper.MapToEmployeeDto(employee);

        var employeeProjects = await _employeeRepository.GetProjectEmployeesByEmployeeIdAsync(employee.Id);
        foreach (var employeeProject in employeeProjects)
        {
            employeeDto.ProjectEmployees.Add(CustomMapper.MapToProjectEmployeeDto(employeeProject));
        }
        return employeeDto;
    }

    public async Task<GetEmployeeDto> AddEmployeeAsync(AddEmployeeDto newEmployeeDto)
    {
        var employee = CustomMapper.MapToEmployee(newEmployeeDto);
        var createdEmployee = await _employeeRepository.AddEmployeeAsync(employee);
        return CustomMapper.MapToEmployeeDto(createdEmployee);
    }

    public async Task<GetEmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto updatedEmployeeDto)
    {
        var employee = CustomMapper.MapToEmployee(id, updatedEmployeeDto);
        var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(employee);
        return CustomMapper.MapToEmployeeDto(updatedEmployee);
    }

    public async Task<List<Subdivision>> GetAllSubdivisionsAsync()
    {
        return await _employeeRepository.GetAllSubdivisionsAsync();
    }

    public async Task<List<Position>> GetAllPositionsAsync()
    {
        return await _employeeRepository.GetAllPositionsAsync();
    }

    public async Task<List<EmployeeStatus>> GetAllStatusesAsync()
    {
        return await _employeeRepository.GetAllStatusesAsync();
    }

    public async Task<List<GetEmployeeDto>> GetAllEmployeesByPositionAsync(string positionName)
    {
        var employees = await _employeeRepository.GetAllEmployeesByPositionAsync(positionName);
        return employees.Select(e => CustomMapper.MapToEmployeeDto(e)).ToList();
    }
}

