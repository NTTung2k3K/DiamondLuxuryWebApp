using DiamondLuxurySolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class UpdateOrderRequest
    {
        public string OrderId { get; set; }

        public string ShipName { get; set; }

        public Guid? DiscountId { get; set; }

        public Guid? ShipperId { get; set; }

        public string ShipPhoneNumber { get; set; }

        public string ShipEmail { get; set; }

        public string ShipAdress { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal ShipPrice { get; set; }

        public string Status { get; set; }

        public Guid CustomerId { get; set; }
        public virtual ICollection<CampaignDetail> CampaignDetails { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
