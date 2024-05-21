using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Payment
{
    public Guid PaymentId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? Description { get; set; }

    public string? Message { get; set; }

    public DateTime PaymentTime { get; set; }

    public bool Status { get; set; }

    public string? OrderId { get; set; }

    public virtual Order? Order { get; set; }
}
