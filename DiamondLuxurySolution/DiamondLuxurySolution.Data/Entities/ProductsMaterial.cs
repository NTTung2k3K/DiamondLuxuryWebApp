using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class ProductsMaterial
{
    public Guid MaterialId { get; set; }

    public string ProductId { get; set; } = null!;

    public int Weight { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
