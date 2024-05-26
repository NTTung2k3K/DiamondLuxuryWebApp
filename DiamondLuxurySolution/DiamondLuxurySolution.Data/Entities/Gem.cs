using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Gem
{
    public Guid GemId { get; set; }

    public string GemName { get; set; } = null!;

    public string? ProportionImage { get; set; }

    public string? Symetry { get; set; }

    public string? Polish { get; set; }

    public bool IsOrigin { get; set; }

<<<<<<< HEAD

=======
>>>>>>> 59348e02b106350021dbba36fe0bb84fc3d839e4
    public bool Fluoresence { get; set; }

    public string? GemImage { get; set; }
    public DateTime AcquisitionDate { get; set; }

    public bool Active { get; set; }
    public int MainGemQuantity { get; set; }
    public virtual ICollection<GemPriceList> GemPriceLists
    { get; set; } = new List<GemPriceList>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
