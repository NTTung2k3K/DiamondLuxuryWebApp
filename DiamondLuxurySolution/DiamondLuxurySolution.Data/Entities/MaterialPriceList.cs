using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class MaterialPriceList
{
    public int MaterialPriceListId { get; set; }

    public decimal BuyPrice { get; set; }

    public decimal SellPrice { get; set; }

    public virtual ICollection<MaterialPriceListDetail> MaterialPriceListDetails { get; set; } = new List<MaterialPriceListDetail>();
}
