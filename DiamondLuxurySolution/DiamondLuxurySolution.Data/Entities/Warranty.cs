using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Warranty
{
    public string WarrantyId { get; set; }

    public string? WarrantyName { get; set; }

    public string? Description { get; set; }

    public DateTime DateActive { get; set; }

    public DateTime DateExpired { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<WarrantyDetail> WarrantyDetails { get; set; } = new List<WarrantyDetail>();

}
