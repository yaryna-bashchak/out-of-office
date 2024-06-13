namespace OutOfOffice.Contracts.Models;

public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int AbsenceReasonId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int RequestTypeId { get; set; }
    public int? Hours { get; set; }
    public int StatusId { get; set; }
    public string Comment { get; set; }

    public AbsenceReason AbsenceReason { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
    public RequestType RequestType { get; set; } = null!;
    public LeaveRequestStatus Status { get; set; } = null!;
}
