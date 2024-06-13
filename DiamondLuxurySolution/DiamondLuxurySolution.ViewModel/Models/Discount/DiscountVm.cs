using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Discount
{
    public class DiscountVm
    {
        public string DiscountId { get; set; }

        public string DiscountName { get; set; }

        public string Description { get; set; }

        public double PercentSale { get; set; }

		public int From { get; set; }
		public int To { get; set; }

		public bool Status { get; set; }
    }
}
