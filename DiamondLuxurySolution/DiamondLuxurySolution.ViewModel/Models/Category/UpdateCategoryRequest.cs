using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Category
{
    public class UpdateCategoryRequest
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? CategoryType { get; set; }

        public IFormFile? CategoryImage { get; set; }

        public bool Status { get; set; }
    }
}
