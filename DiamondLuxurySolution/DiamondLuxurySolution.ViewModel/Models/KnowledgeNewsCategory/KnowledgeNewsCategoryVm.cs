using DiamondLuxurySolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory
{
    public class KnowledgeNewsCategoryVm
    {
        public int KnowledgeNewCatagoryId { get; set; }

        public string KnowledgeNewCatagoriesName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
