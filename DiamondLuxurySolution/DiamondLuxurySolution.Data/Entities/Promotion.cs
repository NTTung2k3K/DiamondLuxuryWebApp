using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Promotion
{
    public Guid PromotionId { get; set; }

    public string? PromotionName { get; set; }

    public string? Description { get; set; }

    public string? PromotionImage { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    public string? BannerImage { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? MaxDiscount { get; set; }

    public bool Status { get; set; }

    public List<Order> Orders { get; set; } = new List<Order>();

}
