using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Slide
{
    public int SlideId { get; set; }

    public string SlideName { get; set; } = null!;

    public string? Description { get; set; }

    public string SlideUrl { get; set; } = null!;

    public string SlideImage { get; set; } = null!;

    public bool Status { get; set; }
}
