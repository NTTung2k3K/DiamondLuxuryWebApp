using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Collection
{
    public string CollectionId { get; set; } = null!;

    public string CollectionName { get; set; } = null!;

    public string? Description { get; set; }

    public string Thumbnail { get; set; } = null!;

    public virtual ICollection<ProductsCollection> ProductsCollections { get; set; } = new List<ProductsCollection>();
}
