using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Collection
{
    public class CreateCollectionRequest
    {
        [Required(ErrorMessage = "Cần đặt tên cho bộ sưu tập")]
        public string? CollectionName { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Cần hình bộ sưu tập")]
        public IFormFile? Thumbnail { get; set; }

        public bool Status { get; set; }
        public bool IsHome { get; set; }
        public string? priceDisplay { get; set; }

        public virtual ICollection<string> ListProductId { get; set; } = new List<string>();
    }
}
