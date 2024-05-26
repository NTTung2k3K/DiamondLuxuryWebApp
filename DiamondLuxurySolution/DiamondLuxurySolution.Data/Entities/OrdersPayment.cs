using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public class OrdersPayment
    {
        public Guid OrdersPaymentId { get; set; }
        public string OrderId { get; set; } = null!;

        public Guid PaymentId { get; set; }

        public string? Message { get; set; }

        public DateTime PaymentTime { get; set; }

        public bool Status { get; set; }

        public Order Order { get; set; } = null!;
        public Payment Payment { get; set; } = null!;
    }
}
