using DiamondLuxurySolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class CreateOrderRequest
    {
        public string OrderId { get; set; } = null!;

        public string ShipName { get; set; } = null!;

        public Guid? DiscountId { get; set; }

        public Guid? ShipperId { get; set; }

        public string ShipPhoneNumber { get; set; } = null!;

        public string ShipEmail { get; set; } = null!;

        public string ShipAdress { get; set; }

        public DateTime OrderDate { get; set; }


        public decimal ShipPrice { get; set; }

        public string Status { get; set; }

        public Guid CustomerId { get; set; }

        public virtual ICollection<CampaignDetail> CampaignDetails { get; set; } = new List<CampaignDetail>();

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}
