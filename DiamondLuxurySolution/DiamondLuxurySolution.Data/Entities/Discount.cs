using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Discount
{
    public string DiscountId { get; set; }

    public string? DiscountName { get; set; }

    public string? Description { get; set; }

    public double PercentSale { get; set; }
    public int From { get; set; }
    public int To { get; set; }

    
    public bool Status { get; set; }
    public List<Order> Orders { get; set; }
    
}
