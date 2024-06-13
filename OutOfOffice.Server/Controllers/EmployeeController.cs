using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetEmployeeDto>>> GetAllEmployeesAsync()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        if (employees == null || employees.Count == 0)
        {
            return NotFound("No employees found.");
        }
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetEmployeeDto>> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound($"Employee with ID {id} not found.");
        }
        return Ok(employee);
    }
}
