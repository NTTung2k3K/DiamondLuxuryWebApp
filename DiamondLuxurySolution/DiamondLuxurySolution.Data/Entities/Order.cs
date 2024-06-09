using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Order
{
    public string OrderId { get; set; } = null!;
    public string? DiscountId { get; set; }

    public string ShipName { get; set; } = null!;
    public bool isShip {  get; set; }
    public Guid? ShipperId { get; set; }
    public Guid? StaffId { get; set; }
    public string? Description { get; set; }

    public string ShipPhoneNumber { get; set; } = null!;

    public string ShipEmail { get; set; } = null!;

    public string ShipAdress { get; set; }

    public DateTime OrderDate { get; set; }
    public DateTime? Datemodified { get; set; }

    public decimal? TotalSale { get; set; }

    public decimal TotalAmout { get; set; }

    public decimal Deposit {  get; set; }
    public decimal RemainAmount { get; set; }

    public string Status { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? PromotionId { get; set; }
    public Promotion? Promotion { get; set; }
    public virtual Discount? Discount { get; set; }
    public virtual AppUser Customer { get; set; }
    public virtual AppUser? Staff { get; set; }


    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrdersPayment> OrdersPayment { get; set; } = new List<OrdersPayment>();

    public virtual AppUser? Shipper { get; set; }

}
