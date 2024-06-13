using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class UpdateShipperRequest
    {
        public string OrderId { get; set; }
        public Guid ShipperId { get; set; }
    }
}
