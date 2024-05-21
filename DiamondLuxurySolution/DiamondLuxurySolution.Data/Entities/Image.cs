using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Image
{
    public int ImageId { get; set; }

    public string? Description { get; set; }

    public string ImagePath { get; set; } = null!;

    public string? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
