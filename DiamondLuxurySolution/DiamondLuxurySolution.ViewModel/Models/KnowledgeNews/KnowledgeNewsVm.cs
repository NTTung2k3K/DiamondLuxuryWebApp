using DiamondLuxurySolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.KnowledgeNews
{
    public class KnowledgeNewsVm
    {
        public int KnowledgeNewsId { get; set; }

        public string? KnowledgeNewsName { get; set; }

        public string? Thumnail { get; set; }

        public string? Description { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
        public bool Active { get; set; }
        public KnowledgeNewCatagory? KnowledgeNewCatagory { get; set; }
        public virtual AppUser? Writer { get; set; }
    }
}
