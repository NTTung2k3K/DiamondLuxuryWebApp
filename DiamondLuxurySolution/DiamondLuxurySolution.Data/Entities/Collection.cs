using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Collection
{
    public string CollectionId { get; set; } = null!;

    public string CollectionName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Thumbnail { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<ProductsCollection> ProductsCollections { get; set; } = new List<ProductsCollection>();
}
