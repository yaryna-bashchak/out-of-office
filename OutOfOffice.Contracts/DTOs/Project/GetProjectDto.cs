using OutOfOffice.Contracts.DTOs.Project;
using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Contracts.DTOs;

public class GetProjectDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Comment { get; set; }
    public GetEmployeeDto ProjectManager { get; set; }
    public ProjectType ProjectType { get; set; }
    public ProjectStatus Status { get; set; }
    public List<ProjectEmployeeDto> ProjectEmployees { get; set; }
}
