using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.KnowledgeNews
{
    public class CreateKnowledgeNewsRequest
    {
        public string KnowledgeNewsName { get; set; } = null!;

        public IFormFile? Thumnail { get; set; }

        public string? Description { get; set; }

        public bool Active { get; set; }
        public Guid WriterId { get; set; }
        public int KnowledgeNewCatagoryId { get; set; }
    }
}
