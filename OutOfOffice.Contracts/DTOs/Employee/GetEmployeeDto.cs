﻿using OutOfOffice.Contracts.DTOs.Project;
using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Contracts.DTOs;

public class GetEmployeeDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public decimal OutOfOfficeBalance { get; set; }
    public PeoplePartnerDto PeoplePartner { get; set; }
    public Position Position { get; set; }
    public EmployeeStatus Status { get; set; }
    public Subdivision Subdivision { get; set; }
    public List<ProjectEmployeeDto> ProjectEmployees { get; set; }
}
