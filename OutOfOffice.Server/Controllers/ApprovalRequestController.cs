using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalRequestController : ControllerBase
{
    private readonly IApprovalRequestService _approvalRequestService;

    public ApprovalRequestController(IApprovalRequestService approvalRequestService)
    {
        _approvalRequestService = approvalRequestService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetApprovalRequestDto>>> GetAllApprovalRequestsAsync()
    {
        try
        {
            var approvalRequests = await _approvalRequestService.GetAllApprovalRequestsAsync();
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
}
