namespace OutOfOffice.Contracts.Models;

public class Project
{
    public int Id { get; set; }
    public int ProjectTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int ProjectManagerId { get; set; }
    public int StatusId { get; set; }
    public string Comment { get; set; }

    public Employee ProjectManager { get; set; }
    public ProjectType ProjectType { get; set; }
    public ProjectStatus Status { get; set; }
}
