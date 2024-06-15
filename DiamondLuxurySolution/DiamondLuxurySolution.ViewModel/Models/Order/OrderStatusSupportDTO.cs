using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class OrderStatusSupportDTO
    {
        public Guid? OrderPaymentId { get; set; }
        public string? Status { get; set; }
    }
}
