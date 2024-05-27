using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class ChangeOrderStatusRequest
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
    }
}
