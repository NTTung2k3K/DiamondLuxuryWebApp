using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Promotion
{
    public class UpdatePromotionRequest
    {
        public Guid PromotionId { get; set; }

        [Required(ErrorMessage = "Cần nhập tên khuyến mãi")]
        public string? PromotionName { get; set; }

        public string? Description { get; set; }

        public IFormFile? PromotionImage { get; set; }

        [Required(ErrorMessage = "Cần nhập ngày bắt đầu")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Cần nhập ngày kết thúc")]
        public DateTime? EndDate { get; set; }

        public IFormFile? BannerImage { get; set; }

        [Required(ErrorMessage = "Cần nhập phần trăm thanh toán")]
        public string? DiscountPercent { get; set; }

        [Required(ErrorMessage = "Cần nhập giảm giá tối đa")]
        public string? MaxDiscount { get; set; }

        public bool Status { get; set; }
    }
}
