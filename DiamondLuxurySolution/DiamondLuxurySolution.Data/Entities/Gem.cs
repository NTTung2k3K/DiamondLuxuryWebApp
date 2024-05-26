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
>>>>>>> 82106bed7b2dca6b6a9cc681d7adbedf5ad8dd6a
    public bool Fluoresence { get; set; }

    public string? GemImage { get; set; }
    public DateTime AcquisitionDate { get; set; }

    public bool Active { get; set; }
    public virtual ICollection<GemPriceList> GemPriceLists
    { get; set; } = new List<GemPriceList>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
