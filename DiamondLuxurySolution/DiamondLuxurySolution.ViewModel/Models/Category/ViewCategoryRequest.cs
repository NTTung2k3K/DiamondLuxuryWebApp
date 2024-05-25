using DiamondLuxurySolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Category
{
    public class ViewCategoryRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
    }
}
