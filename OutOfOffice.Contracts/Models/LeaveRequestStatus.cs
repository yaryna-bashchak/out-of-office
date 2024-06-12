using System;
using System.Collections.Generic;

namespace OutOfOffice.Contracts.Models;

public class LeaveRequestStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
