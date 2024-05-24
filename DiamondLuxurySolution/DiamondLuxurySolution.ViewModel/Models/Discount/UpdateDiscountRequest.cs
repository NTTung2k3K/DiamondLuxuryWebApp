using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Discount
{
    public class UpdateDiscountRequest
    {
        public Guid DiscountId { get; set; }

        public string DiscountName { get; set; } = null!;

        public string? Description { get; set; }

        public IFormFile DiscountImage { get; set; } = null!;

        public double PercentSale { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Status { get; set; }
    }
}
