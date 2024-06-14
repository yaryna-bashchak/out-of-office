using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Contracts.DTOs;

public class UpdateLeaveRequestDto
{
    [Required(ErrorMessage = "StartDate is required")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required")]
    public DateTime EndDate { get; set; }

    public int? Hours { get; set; }
    public string Comment { get; set; }
    public int AbsenceReasonId { get; set; }
    public int EmployeeId { get; set; }
    public int RequestTypeId { get; set; }
}
