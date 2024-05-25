using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class WareHouse
{
    public int WareHouseId { get; set; }

    public string WareHouseName { get; set; } = null!;

    public string? Description { get; set; }

    public string Location { get; set; } = null!;

    public virtual ICollection<ProductsWareHouse> ProductsWareHouses { get; set; } = new List<ProductsWareHouse>();
}
