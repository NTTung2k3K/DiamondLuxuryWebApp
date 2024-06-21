using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
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
        public KnowledgeNewsCategoryVm? KnowledgeNewCatagoryVm { get; set; }
        public virtual StaffVm? Writer { get; set; }
	}
}
