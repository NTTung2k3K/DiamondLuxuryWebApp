using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Material
{
    public Guid MaterialId { get; set; }

    public string MaterialName { get; set; } = null!;

    public string? Description { get; set; }

    public string SubMaterial { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual ICollection<MaterialPriceListDetail> MaterialPriceListDetails { get; set; } = new List<MaterialPriceListDetail>();

    public virtual ICollection<ProductsMaterial> ProductsMaterials { get; set; } = new List<ProductsMaterial>();
}
