namespace OutOfOffice.Contracts.Models;

public class ApprovalRequest
{
    public int Id { get; set; }
    public int ApproverId { get; set; }
    public int LeaveRequestId { get; set; }
    public int StatusId { get; set; }
    public string Comment { get; set; }

    public Employee Approver { get; set; }
    public LeaveRequest LeaveRequest { get; set; }
    public ApprovalRequestStatus Status { get; set; }
}
