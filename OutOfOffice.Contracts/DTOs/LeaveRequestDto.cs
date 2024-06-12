using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Contracts.DTOs;

public class LeaveRequestDto
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Hours { get; set; }
    public string Comment { get; set; }
    public AbsenceReason AbsenceReason { get; set; } = null!;
    public EmployeeDto Employee { get; set; } = null!;
    public RequestType RequestType { get; set; } = null!;
    public LeaveRequestStatus Status { get; set; } = null!;
}
