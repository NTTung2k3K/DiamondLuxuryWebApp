using DiamondLuxurySolution.ViewModel.Models.Category;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Product
{
    public class ProductVm
    {
        public string ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }

        public string ProductThumbnail { get; set; }

        public bool IsHome { get; set; }

        public bool IsSale { get; set; }

        public decimal ProcessingPrice { get; set; }

        public int PercentSale { get; set; }

        public int? CategoryId { get; set; }

        public string Status {  get; set; }
        public int Quantity {  get; set; }

        public virtual ICollection<String> Images { get; set; } = new List<String>();
        public virtual ICollection<SubGemSupportDTO> ListSubGems { get; set; } = new List<SubGemSupportDTO>();

        public virtual GemVm GemVm { get; set; }
        public virtual CategoryVm CategoryVm { get; set; }
        public virtual MaterialVm MaterialVm { get; set; }

        public virtual FrameVm FrameVm { get; set; }

    }
}
