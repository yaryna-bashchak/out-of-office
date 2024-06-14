using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    public ProjectController(IProjectService projectService)
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
        var validationResult = ValidateProject(project.StartDate, project.EndDate);
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
        var validationResult = ValidateProject(project.StartDate, project.EndDate);
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

    private ActionResult ValidateProject(DateTime startDate, DateTime? endDate)
    {
        if (endDate != null && endDate < startDate)
            return BadRequest("EndDate must be greater than or equal to StartDate.");

        return null;
    }
}
