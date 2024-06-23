using DiamondLuxurySolution.Data.Configurations;
using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Gem
{
    public Guid GemId { get; set; }

    public string? GemName { get; set; }

    public string? ProportionImage { get; set; }

    public string? Symetry { get; set; }

    public string? Polish { get; set; }

    public bool IsOrigin { get; set; }

    public bool Fluoresence { get; set; }

    public string? GemImage { get; set; }
    public DateTime AcquisitionDate { get; set; }

    public bool Active { get; set; }
    public string InspectionCertificateId { get; set; }
    public InspectionCertificate InspectionCertificate { get; set; }

	public int GemPriceListId { get; set; }
	public GemPriceList GemPriceList { get; set; }
	public virtual ICollection<Product> Products { get; set; } = new List<Product>();

}
