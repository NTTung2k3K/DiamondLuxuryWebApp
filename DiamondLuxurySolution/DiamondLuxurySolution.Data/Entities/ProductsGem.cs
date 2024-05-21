using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class ProductsGem
{
    public Guid GemId { get; set; }

    public string ProductId { get; set; } = null!;

    public decimal MainGemPrice { get; set; }

    public decimal SubGemPrice { get; set; }

    public virtual Gem Gem { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
