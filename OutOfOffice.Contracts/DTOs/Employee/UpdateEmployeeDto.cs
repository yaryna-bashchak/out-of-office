using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Contracts.DTOs;

public class UpdateEmployeeDto
{
    [Required(ErrorMessage = "FullName is required")]
    public string FullName { get; set; }
    public decimal OutOfOfficeBalance { get; set; }
    public int PeoplePartnerId { get; set; }
    public int PositionId { get; set; }
    public int StatusId { get; set; }
    public int SubdivisionId { get; set; }
}
