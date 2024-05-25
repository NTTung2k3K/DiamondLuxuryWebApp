using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Material
{
    public Guid MaterialId { get; set; }

    public string MaterialName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Color { get; set; }
    public int Weight { get; set; }

    public string? MaterialImage { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Product> Products{ get; set; } = new List<Product>();

    public virtual ICollection<MaterialPriceList> MaterialPriceLists { get; set; } = new List<MaterialPriceList>();

}
