using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class ProductsCollection
{
    public string CollectionId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Collection Collection { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
