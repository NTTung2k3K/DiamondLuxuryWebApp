using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Gem
{
    public Guid GemId { get; set; }

    public string ProportionImage { get; set; } = null!;

    public string Symetry { get; set; } = null!;

    public string Polish { get; set; } = null!;

    public decimal Price { get; set; }

    public bool IsOrigin { get; set; }

    public bool _4c { get; set; }

    public bool IsMain { get; set; }

    public bool Fluoresence { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<GemPriceListDetail> GemPriceListDetails { get; set; } = new List<GemPriceListDetail>();

    public virtual ICollection<ProductsGem> ProductsGems { get; set; } = new List<ProductsGem>();
}
