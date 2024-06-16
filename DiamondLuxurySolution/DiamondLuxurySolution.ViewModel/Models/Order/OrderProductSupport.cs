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
		public int? Size { get; set; }
        public int? OldSize { get; set; }
        public override bool Equals(object obj)
		{
			if (obj is OrderProductSupport other)
			{
				if (Size.HasValue && other.Size.HasValue)
				{
					return ProductId == other.ProductId && Size == other.Size;
				}
				return ProductId == other.ProductId;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int hash = ProductId.GetHashCode();
			if (Size.HasValue)
			{
				hash = (hash * 397) ^ Size.Value.GetHashCode();
			}
			return hash;
		}
	}       
}
