using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Discount
{
    public Guid DiscountId { get; set; }

    public string DiscountName { get; set; } = null!;

    public string? Description { get; set; }

    public double PercentSale { get; set; }

    public bool Status { get; set; }
    public List<Order> Orders { get; set; }
    
}
