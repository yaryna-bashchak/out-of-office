using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalRequestsController : ControllerBase
{
    private readonly IApprovalRequestService _approvalRequestService;

    public ApprovalRequestsController(IApprovalRequestService approvalRequestService)
    {
        _approvalRequestService = approvalRequestService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetApprovalRequestDto>>> GetAllApprovalRequestsAsync([FromQuery] string searchTerm = null)
    {
        try
        {
            var approvalRequests = await _approvalRequestService.GetAllApprovalRequestsAsync(searchTerm);
            return Ok(approvalRequests);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetApprovalRequestDto>> GetApprovalRequestByIdAsync(int id)
    {
        try
        {
            var approvalRequest = await _approvalRequestService.GetApprovalRequestByIdAsync(id);
            return Ok(approvalRequest);
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

    [HttpPut("{id}")]
    public async Task<ActionResult<GetApprovalRequestDto>> UpdateApprovalRequestAsync(int id, [FromBody] UpdateApprovalRequestDto updatedApprovalRequestDto)
    {
        try
        {
            var updatedApprovalRequest = await _approvalRequestService.UpdateApprovalRequestAsync(id, updatedApprovalRequestDto);
            return Ok(updatedApprovalRequest);
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

    [HttpGet("statuses")]
    public async Task<ActionResult<List<ApprovalRequestStatus>>> GetAllStatusesAsync()
    {
        try
        {
            var statuses = await _approvalRequestService.GetAllStatusesAsync();
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
