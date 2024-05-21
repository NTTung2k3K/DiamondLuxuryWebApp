using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class GemPriceListDetail
{
    public Guid GemId { get; set; }

    public int GemPriceListId { get; set; }

    public DateTime EffectDate { get; set; }

    public virtual Gem Gem { get; set; } = null!;

    public virtual GemPriceList GemPriceList { get; set; } = null!;
}
