using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Category
{
    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; } = null!;

        public string? CategoryType { get; set; }

        public IFormFile? CategoryImage { get; set; }
        public decimal CategoryPriceProcessing { get; set; }
    }
}
