namespace OutOfOffice.Contracts.Models;

public class ProjectEmployee
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Employee Employee { get; set; }
    public Project Project { get; set; }
}
