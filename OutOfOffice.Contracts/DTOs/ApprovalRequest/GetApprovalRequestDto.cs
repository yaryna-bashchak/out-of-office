using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Contracts.DTOs;

public class GetApprovalRequestDto
{
    public int Id { get; set; }
    public string Comment { get; set; }
    public GetEmployeeDto Approver { get; set; }
    public GetLeaveRequestDto LeaveRequest { get; set; }
    public ApprovalRequestStatus Status { get; set; }
}
