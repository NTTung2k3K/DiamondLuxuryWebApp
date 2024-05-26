using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Category
{
    public class CategoryVm
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? CategoryType { get; set; }

        public string? CategoryImage { get; set; }
        public decimal CategoryPriceProcessing { get; set; }
        public bool Status { get; set; }
    }
}
