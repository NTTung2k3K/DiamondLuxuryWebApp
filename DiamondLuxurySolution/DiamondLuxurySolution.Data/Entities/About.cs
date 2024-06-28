using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class About
{
    public int AboutId { get; set; }
    public string? AboutName { get; set; }
    public string? AboutEmail { get; set; }
    public string? AboutAddress { get; set; }
    public string? AboutPhoneNumber { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; }

}
