namespace OutOfOffice.Contracts.DTOs;

public class AddEmployeeDto
{
    public string FullName { get; set; }
    public decimal OutOfOfficeBalance { get; set; }
    // public byte[] Photo { get; set; }
    public int? PeoplePartnerId { get; set; }
    public int PositionId { get; set; }
    public int StatusId { get; set; }
    public int SubdivisionId { get; set; }
}
