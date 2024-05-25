using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Collection
{
    public class UpdateCollectionRequest
    {
        public string CollectionId { get; set; } = null!;

        public string CollectionName { get; set; } = null!;

        public string? Description { get; set; }

        public IFormFile? Thumbnail { get; set; }

        public bool Status { get; set; }
    }
}
