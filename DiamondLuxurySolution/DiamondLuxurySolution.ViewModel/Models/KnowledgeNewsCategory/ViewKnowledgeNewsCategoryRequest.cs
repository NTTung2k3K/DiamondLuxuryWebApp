using DiamondLuxurySolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory
{
    public class ViewKnowledgeNewsCategoryRequest : PagingRequestBase
    {
        public string? KeyWord {  get; set; }
    }
}
