using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Platform
{
    public int PlatformId { get; set; }

    public string PlatformName { get; set; } = null!;

    public string PlatformUrl { get; set; } = null!;

    public string PlatformLogo { get; set; } = null!;

    public bool Status { get; set; }
}
