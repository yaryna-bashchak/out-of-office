using System;
using System.Collections.Generic;

namespace OutOfOffice.Contracts.Models;

public class ApprovalRequest
{
    public int Id { get; set; }
    public int ApproverId { get; set; }
    public int LeaveRequestId { get; set; }
    public int StatusId { get; set; }
    public string? Comment { get; set; }

    public Employee Approver { get; set; } = null!;
    public LeaveRequest LeaveRequest { get; set; } = null!;
    public ApprovalRequestStatus Status { get; set; } = null!;
}
