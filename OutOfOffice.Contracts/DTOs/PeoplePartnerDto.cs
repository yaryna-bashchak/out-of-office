using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Contracts.DTOs;

public class PeoplePartnerDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
}
