using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Contracts.DTOs;

public class AddEmployeeDto
{
    [Required(ErrorMessage = "FullName is required")]
    public string FullName { get; set; }

    public decimal OutOfOfficeBalance { get; set; }

    public int? PeoplePartnerId { get; set; }
    // public byte[] Photo { get; set; }

    [Required(ErrorMessage = "PositionId is required")]
    public int PositionId { get; set; }

    [Required(ErrorMessage = "StatusId is required")]
    public int StatusId { get; set; }

    [Required(ErrorMessage = "SubdivisionId is required")]
    public int SubdivisionId { get; set; }
}
