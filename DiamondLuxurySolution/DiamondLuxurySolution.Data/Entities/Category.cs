using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategoryType { get; set; } = null!;

    public string CategoryImage { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
