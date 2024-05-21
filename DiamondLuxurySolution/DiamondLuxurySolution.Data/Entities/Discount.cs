using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Discount
{
    public Guid DiscountId { get; set; }

    public string DiscountName { get; set; } = null!;

    public string? Description { get; set; }

    public string DiscountImage { get; set; } = null!;

    public double PercentSale { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
