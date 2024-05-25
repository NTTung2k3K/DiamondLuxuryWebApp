using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.KnowledgeNews
{
    public class UpdateKnowledgeNewsRequest
    {
        public int KnowledgeNewsId { get; set; }

        public string KnowledgeNewsName { get; set; } = null!;

        public IFormFile? Thumnail { get; set; }

        public string? Description { get; set; }

        public DateTime DateModified { get; set; }
        public bool Active { get; set; }
    }
}
