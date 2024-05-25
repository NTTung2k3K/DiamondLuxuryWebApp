using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Gem
{
    public Guid GemId { get; set; }

    public string GemName { get; set; } = null!;

    public string? ProportionImage { get; set; }

    public string? Symetry { get; set; }

    public string? Polish { get; set; }

    public decimal Price { get; set; }

    public bool IsOrigin { get; set; }

    public bool IsMain { get; set; }

    public bool Fluoresence { get; set; }
    public DateTime AcquisitionDate { get; set; }

    public bool Active { get; set; }
    public virtual ICollection<GemPriceList> GemPriceLists
    { get; set; } = new List<GemPriceList>();
    public virtual ICollection<ProductsGem> ProductsGems { get; set; } = new List<ProductsGem>();
}
