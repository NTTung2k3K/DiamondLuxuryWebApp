using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class CampaignDetail
{
    public Guid PromotionId { get; set; }

    public string OrderId { get; set; } = null!;

    public double DiscountPercentage { get; set; }

    public decimal FromAmount { get; set; }

    public decimal ToAmount { get; set; }

    public decimal MaxDiscount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Promotion Promotion { get; set; } = null!;
}
