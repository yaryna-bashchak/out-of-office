using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Contracts.DTOs;

public class GetLeaveRequestDto
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int? Hours { get; set; }
    public string Comment { get; set; }
    public AbsenceReason AbsenceReason { get; set; }
    public GetEmployeeDto Employee { get; set; }
    public RequestType RequestType { get; set; }
    public LeaveRequestStatus Status { get; set; }
}
