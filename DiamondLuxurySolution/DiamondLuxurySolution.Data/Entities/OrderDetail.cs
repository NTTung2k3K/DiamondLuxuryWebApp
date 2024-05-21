using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class OrderDetail
{
    public string ProductId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public Guid WarrantyId { get; set; }

    public int Quantity { get; set; }

    public int Discount { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Warranty Warranty { get; set; } = null!;
}
