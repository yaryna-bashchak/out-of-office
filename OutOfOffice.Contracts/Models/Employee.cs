namespace OutOfOffice.Contracts.Models;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int SubdivisionId { get; set; }
    public int PositionId { get; set; }
    public int StatusId { get; set; }
    public int PeoplePartnerId { get; set; }
    public decimal OutOfOfficeBalance { get; set; }

    public Employee PeoplePartner { get; set; }
    public Position Position { get; set; }
    public EmployeeStatus Status { get; set; }
    public Subdivision Subdivision { get; set; }
}
