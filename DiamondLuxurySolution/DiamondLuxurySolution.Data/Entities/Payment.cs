using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Payment
{
    public Guid PaymentId { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<OrdersPayment> OrdersPayment { get; set; } = new List<OrdersPayment>();
}
