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
}
