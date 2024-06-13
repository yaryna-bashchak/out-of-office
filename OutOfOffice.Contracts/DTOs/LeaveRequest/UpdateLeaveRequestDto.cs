using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Contracts.DTOs;

public class UpdateLeaveRequestDto
{
    [Required(ErrorMessage = "StartDate is required")]
    public DateOnly StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required")]
    public DateOnly EndDate { get; set; }

    public int? Hours { get; set; }

    public string Comment { get; set; }

    [Required(ErrorMessage = "AbsenceReasonId is required")]
    public int AbsenceReasonId { get; set; }

    [Required(ErrorMessage = "EmployeeId is required")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "RequestTypeId is required")]
    public int RequestTypeId { get; set; }
    
    [Required(ErrorMessage = "StatusId is required")]
    public int StatusId { get; set; }
}
