using DiamondLuxurySolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.GemPriceList
{
    public class ViewGemPriceListRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
        public bool Active { get; set; }

    }
}
