using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Product    
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage ="Sản phẩm phải có tên")]
        [DisplayName("Tên sản phảm")]
        public string ProductName { get; set; }
        [DisplayName("Mô tả sản phảm")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Sản phảm phải có ảnh đại diện")]
        [DisplayName("Ảnh đại diện")]
        public IFormFile ProductThumbnail { get; set; }
        [DisplayName("Hiện thị ở trang chủ")]
        public bool IsHome { get; set; }
        [DisplayName("Hiện thị ở dạng giảm giá")]
        public bool IsSale { get; set; }
        [DisplayName("% giảm giá")]
        [Required(ErrorMessage = "Sản phẩm phải có % giảm giá (có thể bằng 0)")]
        [Range(0, 100, ErrorMessage = "% giảm giá phải nằm trong khoảng từ 0 đến 100")]
        public int PercentSale { get; set; }
        [Required(ErrorMessage = "Sản phẩm phải có giá gia công (có thể bằng 0)")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Giá gia công phải co giá lơn hơn hoặc bầng 0")]
        [DisplayName("Giá gia công")]
        public decimal ProcessingPrice { get; set; }

        [DisplayName("Hiển thị")]
        public string Status { get; set; }
        [Required(ErrorMessage = "Sản phẩm cần phải có loại")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Sản phẩm phải có số lượng")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng nên lớn hơn hoặc bằng 0")]
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }
        [DisplayName("Hỉnh ảnh phụ")]
        public virtual List<IFormFile>? Images { get; set; } = new List<IFormFile>();
        [DisplayName("Khung sản phẩm")]
        public string? FrameId { get; set; }
        [Required(ErrorMessage = "Sản phẩm cần phải có kim cương chính")]
        [DisplayName("Kim cương chính")]
        public Guid GemId { get; set; }
        [DisplayName("Kim cương phụ")]
        public virtual List<DiamondLuxurySolution.ViewModel.Models.SubGemSupportDTO>? ListSubGems { get; set; } = new List<SubGemSupportDTO>();
        public string? ListSubGemsJson { get; set; }

    }
}
