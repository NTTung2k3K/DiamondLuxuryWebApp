using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public class OrderDiscount
    {
        public string OrderId { get; set; } = null!;
        public Guid DiscountId { get; set; }
        public string Description { get; set; }
        public virtual Order Order { get; set; } = null!;

        public virtual Discount Discount { get; set; } = null!;
    }
}
