using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Discount
{
    public class DiscountVm
    {
        public string DiscountId { get; set; }

        public string DiscountName { get; set; } = null!;

        public string? Description { get; set; }

        public double PercentSale { get; set; }

        public bool Status { get; set; }
    }
}
