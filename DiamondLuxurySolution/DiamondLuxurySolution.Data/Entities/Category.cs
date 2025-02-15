﻿using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryType { get; set; }

    public string? CategoryImage { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
