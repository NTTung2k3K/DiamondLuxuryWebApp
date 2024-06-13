using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Material
{
    public Guid MaterialId { get; set; }
    public string? MaterialName { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public string? MaterialImage { get; set; }
    public decimal? Price { get; set; }
    public bool Status { get; set; }
    public DateTime EffectDate { get; set; }
    public List<Frame> Frames { get; set; }

}
