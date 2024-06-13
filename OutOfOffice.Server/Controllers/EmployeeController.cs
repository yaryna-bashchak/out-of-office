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

    [HttpPost]
    public async Task<ActionResult<GetEmployeeDto>> AddEmployeeAsync([FromBody] AddEmployeeDto newEmployeeDto)
    {
        if (newEmployeeDto == null)
        {
            return BadRequest("Invalid employee data.");
        }

        var addedEmployee = await _employeeService.AddEmployeeAsync(newEmployeeDto);
        if (addedEmployee == null)
        {
            return StatusCode(500, "Unable to add employee. Please check the details and try again.");
        }

        return Ok(addedEmployee);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GetEmployeeDto>> UpdateEmployeeAsync(int id, [FromBody] UpdateEmployeeDto updatedEmployeeDto)
    {
        if (updatedEmployeeDto == null)
        {
            return BadRequest("Invalid employee data.");
        }

        var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, updatedEmployeeDto);
        if (updatedEmployee == null)
        {
            return NotFound($"No employee found with ID {id} to update.");
        }

        return Ok(updatedEmployee);
    }
}
