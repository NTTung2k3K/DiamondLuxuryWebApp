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

        public string ShipName { get; set; } = null!;

        public string? DiscountId { get; set; }

        public string ShipPhoneNumber { get; set; } = null!;

        public string ShipEmail { get; set; } = null!;

        public string ShipAdress { get; set; }

        public string? Status { get; set; }
        public decimal? Deposit{ get; set; }


        public Guid CustomerId { get; set; }
        public virtual ICollection<OrderProductSupport> ListOrderProduct { get; set; } = new List<OrderProductSupport>();

        public virtual ICollection<Guid>? ListPromotionId { get; set; } = new List<Guid>();

<<<<<<< HEAD
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        /*        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
         *        
        */
=======
        public virtual ICollection<Guid> ListPaymentId { get; set; } = new List<Guid>();
>>>>>>> cc97b0c5ca626e3469d8c24450e54774d316c713

    }
}
