using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Contracts.DTOs;

public class UpdateApprovalRequestDto
{
    public string Comment { get; set; }
    public int ApproverId { get; set; }
    public int LeaveRequestId { get; set; }
    public int StatusId { get; set; }
}
