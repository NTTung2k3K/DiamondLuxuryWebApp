using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class About
{
    public int AboutId { get; set; }

    public string AboutName { get; set; } = null!;

    public string? Description { get; set; }

    public string AboutImage { get; set; } = null!;
}
