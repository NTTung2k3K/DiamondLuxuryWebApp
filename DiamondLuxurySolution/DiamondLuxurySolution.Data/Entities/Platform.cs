using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Platform
{
    public int PlatformId { get; set; }

    public string? PlatformName { get; set; }

    public string? PlatformUrl { get; set; }

    public string? PlatformLogo { get; set; }

    public bool Status { get; set; }
}
