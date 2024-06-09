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

        public override bool Equals(object obj)
        {
            if (obj is OrderProductSupport other)
            {
                return ProductId == other.ProductId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (ProductId).GetHashCode();
        }
    }       
}
