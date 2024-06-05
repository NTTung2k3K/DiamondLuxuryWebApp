using DiamondLuxurySolution.ViewModel.Models.Category;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Product
{
    public class ProductVm
    {
        [DisplayName("Id của sản phẩm")]
        public string ProductId { get; set; }
        [DisplayName("Tên sản phảm")]
        public string ProductName { get; set; }
        [DisplayName("Mô tả sản phảm")]
        public string? Description { get; set; }
        [DisplayName("Ảnh đại diện")]
        public string ProductThumbnail { get; set; }
        [DisplayName("Hiện thị ở trang chủ")]
        public bool IsHome { get; set; }
        [DisplayName("Hiện thị ỏ dạng giảm giá")]
        public bool IsSale { get; set; }
        [DisplayName("Giá gia công")]
        public decimal ProcessingPrice { get; set; }
        [DisplayName("% giảm")]
        public int PercentSale { get; set; }
        [DisplayName("Hiển thị")]
        public string Status {  get; set; }
        [DisplayName("Sô lượng")]
        public int Quantity {  get; set; }

        public virtual ICollection<String> Images { get; set; } = new List<String>();
        public virtual ICollection<SubGemSupportDTO> ListSubGems { get; set; } = new List<SubGemSupportDTO>();

        public virtual GemVm GemVm { get; set; }
        public virtual CategoryVm CategoryVm { get; set; }
        public virtual MaterialVm MaterialVm { get; set; }

        public virtual FrameVm FrameVm { get; set; }

    }
}
