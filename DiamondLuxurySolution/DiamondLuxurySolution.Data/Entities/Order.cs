using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Order
{
    public string OrderId { get; set; } = null!;

    public string ShipName { get; set; } = null!;

    public Guid? ShipperId { get; set; }

    public string ShipPhoneNumber { get; set; } = null!;

    public string ShipEmail { get; set; } = null!;

    public string? ShipAdress { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmout { get; set; }

    public decimal ShipPrice { get; set; }

    public bool Status { get; set; }

    public Guid? CustomerId { get; set; }

    public virtual ICollection<CampaignDetail> CampaignDetails { get; set; } = new List<CampaignDetail>();

    public virtual AspNetUser? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual AspNetUser? Shipper { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}
