using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class ProductsWareHouse
{
    public int WareHouseId { get; set; }

    public string ProductId { get; set; } = null!;

    public int QuantityInStocks { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual WareHouse WareHouse { get; set; } = null!;
}
