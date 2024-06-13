using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestController : ControllerBase
{
    private readonly ILeaveRequestService _leaveRequestService;

    public LeaveRequestController(ILeaveRequestService leaveRequestService)
    {
        _leaveRequestService = leaveRequestService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetLeaveRequestDto>>> GetAllLeaveRequestsAsync()
    {
        try
        {
            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync();
            return Ok(leaveRequests);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetLeaveRequestDto>> GetLeaveRequestByIdAsync(int id)
    {
        try
        {
            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
            return Ok(leaveRequest);
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
    public async Task<ActionResult<GetLeaveRequestDto>> AddLeaveRequestAsync([FromBody] AddLeaveRequestDto newLeaveRequestDto)
    {
        var validationResult = ValidateLeaveRequest(newLeaveRequestDto.StartDate, newLeaveRequestDto.EndDate, newLeaveRequestDto.Hours);
        if (validationResult != null)
            return validationResult;

        try
        {
            var addedLeaveRequest = await _leaveRequestService.AddLeaveRequestAsync(newLeaveRequestDto);
            return Ok(addedLeaveRequest);
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
    public async Task<ActionResult<GetLeaveRequestDto>> UpdateLeaveRequestAsync(int id, [FromBody] UpdateLeaveRequestDto updatedLeaveRequestDto)
    {
        var validationResult = ValidateLeaveRequest(updatedLeaveRequestDto.StartDate, updatedLeaveRequestDto.EndDate, updatedLeaveRequestDto.Hours);
        if (validationResult != null)
            return validationResult;

        try
        {
            var updatedLeaveRequest = await _leaveRequestService.UpdateLeaveRequestAsync(id, updatedLeaveRequestDto);
            return Ok(updatedLeaveRequest);
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

    private ActionResult ValidateLeaveRequest(DateOnly startDate, DateOnly endDate, int? hours)
    {
        if (endDate < startDate)
            return BadRequest("EndDate must be greater than or equal to StartDate.");

        if (hours.HasValue && (hours < 0 || hours > 8))
            return BadRequest("Hours must be between 0 and 8 hours.");

        return null;
    }
}
