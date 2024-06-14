namespace OutOfOffice.Contracts.DTOs.Project;

public class ProjectEmployeeDto
{
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
