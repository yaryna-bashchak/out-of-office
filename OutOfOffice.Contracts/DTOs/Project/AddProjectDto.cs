using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Contracts.DTOs;

public class AddProjectDto
{
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Comment { get; set; }
    public int ProjectManagerId { get; set; }
    public int ProjectTypeId { get; set; }
    public int StatusId { get; set; }
}
