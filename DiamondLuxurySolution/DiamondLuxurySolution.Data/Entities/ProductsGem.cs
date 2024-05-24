using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class ProductsGem
{
    public Guid GemId { get; set; }

    public string ProductId { get; set; } = null!;

    public int MainGemQuantity { get; set; }

    public int SubGemQuantity { get; set; }

    public virtual Gem Gem { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
