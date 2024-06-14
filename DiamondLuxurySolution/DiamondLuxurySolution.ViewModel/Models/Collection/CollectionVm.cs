using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Collection
{
    public class CollectionVm
    {
        public string CollectionId { get; set; } = null!;

        public string CollectionName { get; set; } = null!;

        public string? Thumbnail { get; set; }

        public bool Status { get; set; }
        public bool WantImgEmpty { get; set; }
        public string? Description { get; set; }

        public ICollection<DiamondLuxurySolution.ViewModel.Models.Product.ProductVm> ListProducts
            = new List<ProductVm>();
        public virtual ICollection<string> ListProductsIdAdd { get; set; } = new List<string>();
        public virtual ICollection<string> ListProductsIdDelete { get; set; } = new List<string>();
    }
}
