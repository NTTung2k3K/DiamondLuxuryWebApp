using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class MaterialPriceList
{
    public int MaterialPriceListId { get; set; }

    public decimal? BuyPrice { get; set; }

    public decimal? SellPrice { get; set; }
    public Guid MaterialId { get; set; }
    public Material Material { get; set; }
    public bool Active {  get; set; }
    public DateTime effectDate { get; set; }

}
