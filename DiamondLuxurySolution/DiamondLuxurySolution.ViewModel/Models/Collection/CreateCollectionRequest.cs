using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Collection
{
    public class CreateCollectionRequest
    {
        public string CollectionName { get; set; } = null!;

        public string? Description { get; set; }

        public IFormFile? Thumbnail { get; set; }

        public bool Status { get; set; }

<<<<<<< HEAD
        public virtual ICollection<string> ListProductId { get; set; } = new List<string>();
=======
        public string OrderId {  get; set; }
>>>>>>> 6a5e8acefe04d73626e1a982a831cc1441d88b38
    }
}
