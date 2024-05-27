using DiamondLuxurySolution.ViewModel.Models.Warranty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class OrderProductSupport
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductThumbnail { get; set; }
        public decimal UnitPrice { get; set; }
        public WarrantyVm? Warranty { get; set; }

       
    }
}
