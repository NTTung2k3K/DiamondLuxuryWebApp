using DiamondLuxurySolution.ViewModel.Models.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Collection
{
    public class UpdateCollectionRequest
    {
        public string CollectionId { get; set; } = null!;

        [Required(ErrorMessage = "Cần đặt tên cho bộ sưu tập")]
        public string? CollectionName { get; set; }

        public string? Description { get; set; }
        public IFormFile? Thumbnail { get; set; }

        public bool Status { get; set; }
        public bool IsHome { get; set; }
        public string? priceDisplay { get; set; }
        public virtual ICollection<string> ListProductsIdAdd { get; set; } = new List<string>();
        public virtual ICollection<string> ListProductsIdDelete { get; set; } = new List<string>();
        public string? ProductId { get; set; }
    }
}
