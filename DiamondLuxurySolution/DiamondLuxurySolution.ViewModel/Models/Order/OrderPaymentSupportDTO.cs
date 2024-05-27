using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class OrderPaymentSupportDTO
    {
        public Guid PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
        public DateTime PaymentTime { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
