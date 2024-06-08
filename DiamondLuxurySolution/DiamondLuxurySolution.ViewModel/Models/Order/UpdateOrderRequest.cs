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
        public string OrderId { get; set; } = null!;

        public string ShipName { get; set; } = null!;

        public string? DiscountId { get; set; }

        public string ShipPhoneNumber { get; set; } = null!;

        public string ShipEmail { get; set; } = null!;

        public string ShipAdress { get; set; }

        public string? Status { get; set; }
        public decimal? Deposit { get; set; }


        public Guid CustomerId { get; set; }
        public virtual ICollection<OrderProductSupport> ListOrderProduct { get; set; } = new List<OrderProductSupport>();

        public Guid? PromotionId { get; set; }


        public virtual ICollection<Guid> ListPaymentId { get; set; } = new List<Guid>();

    }
}

