using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Warranty
{
    public Guid WarrantyId { get; set; }

    public string WarrantyName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime DateActive { get; set; }

    public DateTime DateExpired { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
