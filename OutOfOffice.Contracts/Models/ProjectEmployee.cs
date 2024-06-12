namespace OutOfOffice.Contracts.Models;

public class ProjectEmployee
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public Employee Employee { get; set; } = null!;
    public Project Project { get; set; } = null!;
}
