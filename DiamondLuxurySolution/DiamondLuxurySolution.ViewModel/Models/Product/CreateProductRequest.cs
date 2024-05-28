using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Product
{
    public class CreateProductRequest
    {
        public string? ProductName { get; set; }
        public string? Description { get; set; }

        public IFormFile? ProductThumbnail { get; set; }

        public bool IsHome { get; set; }

        public bool IsSale { get; set; }



        public int PercentSale { get; set; }
        public decimal ProcessingPrice { get; set; }



        public string? Status { get; set; }
        public int? CategoryId { get; set; }

        public int Quantity { get; set; }

        public virtual ICollection<IFormFile>? Images { get; set; } = new List<IFormFile>();
        public string? FrameId { get; set; }

        public Guid? GemId { get; set; }
        public virtual ICollection<DiamondLuxurySolution.ViewModel.Models.SubGemSupportDTO>? ListSubGems { get; set; } = new List<SubGemSupportDTO>();
        public string? ListSubGemsJson { get; set; }

    }
}
