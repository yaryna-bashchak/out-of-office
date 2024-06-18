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

    public AbsenceReason AbsenceReason { get; set; }
    public Employee Employee { get; set; }
    public RequestType RequestType { get; set; }
    public LeaveRequestStatus Status { get; set; }
}
