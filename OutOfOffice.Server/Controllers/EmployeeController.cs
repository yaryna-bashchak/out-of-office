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
        try
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetEmployeeDto>> GetEmployeeByIdAsync(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(employee);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<GetEmployeeDto>> AddEmployeeAsync([FromBody] AddEmployeeDto newEmployeeDto)
    {
        if (newEmployeeDto.OutOfOfficeBalance < 0)
            return BadRequest("OutOfOfficeBalance must be greater than or equals zero.");

        try
        {
            var addedEmployee = await _employeeService.AddEmployeeAsync(newEmployeeDto);
            return Ok(addedEmployee);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GetEmployeeDto>> UpdateEmployeeAsync(int id, [FromBody] UpdateEmployeeDto updatedEmployeeDto)
    {
        if (updatedEmployeeDto.OutOfOfficeBalance < 0)
            return BadRequest("OutOfOfficeBalance must be greater than or equals zero.");

        try
        {
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, updatedEmployeeDto);
            return Ok(updatedEmployee);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
