using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class InspectionCertificate
{
    public string InspectionCertificateId { get; set; } = null!;

    public string? InspectionCertificateName { get; set; }

    public DateTime? DateGrading { get; set; }

    public string? Logo { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
