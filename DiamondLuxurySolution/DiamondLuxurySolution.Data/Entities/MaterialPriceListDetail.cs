using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class MaterialPriceListDetail
{
    public Guid MaterialId { get; set; }

    public int MaterialPriceListId { get; set; }

    public DateTime EffectDate { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual MaterialPriceList MaterialPriceList { get; set; } = null!;
}
