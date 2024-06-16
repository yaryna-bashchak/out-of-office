using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.DTOs.Project;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetProjectDto>>> GetAllProjectsAsync()
    {
        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetProjectDto>> GetProjectByIdAsync(int id)
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            return Ok(project);
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
    public async Task<ActionResult<GetProjectDto>> AddProjectAsync([FromBody] AddProjectDto project)
    {
        var validationResult = ValidateDate(project.StartDate, project.EndDate);
        if (validationResult != null)
            return validationResult;

        try
        {
            var addedProject = await _projectService.AddProjectAsync(project);
            return Ok(addedProject);
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
    public async Task<ActionResult<GetProjectDto>> UpdateProjectAsync(int id, [FromBody] UpdateProjectDto project)
    {
        var validationResult = ValidateDate(project.StartDate, project.EndDate);
        if (validationResult != null)
            return validationResult;

        try
        {
            var updatedProject = await _projectService.UpdateProjectAsync(id, project);
            return Ok(updatedProject);
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

    [HttpPost("addEmployeeToProject")]
    public async Task<ActionResult> AddEmployeeToProjectAsync([FromBody] ProjectEmployeeDto projectEmployeeDto)
    {
        ValidateDate(projectEmployeeDto.StartDate, projectEmployeeDto.EndDate);

        try
        {
            await _projectService.AddEmployeeToProjectAsync(projectEmployeeDto);
            return Ok();
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

    [HttpPut("updateEmployeeInProject")]
    public async Task<ActionResult> UpdateEmployeeInProjectAsync([FromBody] ProjectEmployeeDto projectEmployeeDto)
    {
        ValidateDate(projectEmployeeDto.StartDate, projectEmployeeDto.EndDate);

        try
        {
            await _projectService.UpdateEmployeeInProjectAsync(projectEmployeeDto);
            return Ok();
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

    [HttpGet("statuses")]
    public async Task<ActionResult<List<ProjectStatus>>> GetAllStatusesAsync()
    {
        try
        {
            var statuses = await _projectService.GetAllStatusesAsync();
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("types")]
    public async Task<ActionResult<List<ProjectType>>> GetAllTypesAsync()
    {
        try
        {
            var types = await _projectService.GetAllTypesAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    private ActionResult ValidateDate(DateTime startDate, DateTime? endDate)
    {
        if (endDate != null && endDate < startDate)
            return BadRequest("EndDate must be greater than or equal to StartDate.");

        return null;
    }
}
