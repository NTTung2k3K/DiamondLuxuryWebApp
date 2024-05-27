using DiamondLuxurySolution.ViewModel.Models.Category;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.Warehouse;
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
        public string ProductName { get; set; }
        public string? Description { get; set; }

        public string ProductThumbnail { get; set; }

        public bool IsHome { get; set; }

        public bool IsSale { get; set; }

        public decimal ProcessingPrice { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public int SellingCount { get; set; }

        public int PercentSale { get; set; }

        public int? CategoryId { get; set; }

        public string Status {  get; set; }
        public int Quantity {  get; set; }
        public string? InspectionCertificateId { get; set; }
        public Guid? MaterialId { get; set; }

        public virtual ICollection<String> Images { get; set; } = new List<String>();
        public virtual ICollection<SubGemSupportDTO> ListSubGems { get; set; } = new List<SubGemSupportDTO>();

        public virtual GemVm GemVms{ get; set; }
        public virtual CategoryVm CategoryVm { get; set; }
        public virtual WarehouseVm WareHouseVms { get; set; }
        public virtual MaterialVm MaterialVms { get; set; }
        public virtual InspectionCertificateVm InspectionCertificateVm { get; set; }
    }
}
