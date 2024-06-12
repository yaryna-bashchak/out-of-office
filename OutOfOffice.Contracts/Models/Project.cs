namespace OutOfOffice.Contracts.Models;

public class Project
{
    public int Id { get; set; }
    public int ProjectTypeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int ProjectManagerId { get; set; }
    public int StatusId { get; set; }
    public string Comment { get; set; }

    public Employee ProjectManager { get; set; } = null!;
    public ProjectType ProjectType { get; set; } = null!;
    public ProjectStatus Status { get; set; } = null!;
}
