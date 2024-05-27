using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Promotion
{
    public class CreatePromotionRequest
    {
        public string? PromotionName { get; set; }

        public string? Description { get; set; }

        public IFormFile? PromotionImage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IFormFile? BannerImage { get; set; }

        public string? DiscountPercent { get; set; }

        public string? MaxDiscount { get; set; }

        public bool Status { get; set; }
    }
}
