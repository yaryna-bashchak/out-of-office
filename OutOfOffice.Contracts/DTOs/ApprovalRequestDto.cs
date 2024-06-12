using System;
using System.Collections.Generic;

namespace OutOfOffice.Contracts.DTOs;

public class ApprovalRequestDto
{
    public int Id { get; set; }
    public string? Comment { get; set; }
    public EmployeeDto Approver { get; set; } = null!;
    public LeaveRequest LeaveRequest { get; set; } = null!;
    public ApprovalRequestStatus Status { get; set; } = null!;
}
