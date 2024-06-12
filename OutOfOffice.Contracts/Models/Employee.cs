namespace OutOfOffice.Contracts.Models;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public int SubdivisionId { get; set; }
    public int PositionId { get; set; }
    public int StatusId { get; set; }
    public int? PeoplePartnerId { get; set; }
    public decimal OutOfOfficeBalance { get; set; }
    public byte[]? Photo { get; set; }

    public Employee? PeoplePartner { get; set; }
    public Position Position { get; set; } = null!;
    public EmployeeStatus Status { get; set; } = null!;
    public Subdivision Subdivision { get; set; } = null!;
}
