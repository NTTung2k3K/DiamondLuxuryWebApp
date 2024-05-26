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

        public IFormFile ProductThumbnail { get; set; }

        public bool IsHome { get; set; }

        public bool IsSale { get; set; }

        public decimal ProcessingPrice { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public int SellingCount { get; set; }

        public int PercentSale { get; set; }

        public int CategoryId { get; set; }

        public string? InspectionCertificateId { get; set; }
        public Guid MaterialId { get; set; }

        public virtual ICollection<IFormFile> Images { get; set; } = new List<IFormFile>();

        public virtual ICollection<Guid> Gemid { get; set; } = new List<Guid>();

        public virtual ICollection<int> ProductsWareHouses { get; set; } = new List<int>();
    }
}
