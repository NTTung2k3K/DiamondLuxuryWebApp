using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Slide
{
    public int SlideId { get; set; }

    public string? SlideName { get; set; }

    public string? Description { get; set; }

    public string? SlideUrl { get; set; }

    public string? SlideImage { get; set; }

    public bool Status { get; set; }
}
