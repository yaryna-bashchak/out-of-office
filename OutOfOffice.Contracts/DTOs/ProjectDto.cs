using System;
using System.Collections.Generic;

namespace OutOfOffice.Contracts.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Comment { get; set; }
    public EmployeeDto ProjectManager { get; set; } = null!;
    public ProjectType ProjectType { get; set; } = null!;
    public ProjectStatus Status { get; set; } = null!;
}
