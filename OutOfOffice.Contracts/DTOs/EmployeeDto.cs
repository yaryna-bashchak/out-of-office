using System;
using System.Collections.Generic;

namespace OutOfOffice.Contracts.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public decimal OutOfOfficeBalance { get; set; }
    public byte[]? Photo { get; set; }
    public EmployeeDto? PeoplePartner { get; set; }
    public Position Position { get; set; } = null!;
    public EmployeeStatus Status { get; set; } = null!;
    public Subdivision Subdivision { get; set; } = null!;
}
