using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class CancelOrderRequest
    {
        public string orderId { get; set; }
        public string Description { get; set; }
    }
}
