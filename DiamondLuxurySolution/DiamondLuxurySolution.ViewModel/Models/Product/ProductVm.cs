using DiamondLuxurySolution.Data.Entities;
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


        public string? Status { get; set; }
        public DiamondLuxurySolution.Data.Entities.Category Category { get; set; }

        public DiamondLuxurySolution.Data.Entities.InspectionCertificate InspectionCertificate{ get; set; }

        public DiamondLuxurySolution.Data.Entities.Material Material { get; set; }

        public int? Quantity { get; set; }

        public virtual ICollection<string>? Images { get; set; } = new List<string>();

        public DiamondLuxurySolution.Data.Entities.Gem Gem{ get; set; }

        public virtual ICollection<DiamondLuxurySolution.ViewModel.Models.SubGemSupportDTO>? ListSubGems { get; set; } = new List<SubGemSupportDTO>();

        public DiamondLuxurySolution.Data.Entities.WareHouse WareHouse{ get; set; }

    }
}
