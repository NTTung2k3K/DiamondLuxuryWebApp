using DiamondLuxurySolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.MaterialPriceList
{
    public class ViewMaterialPriceListRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
    }
}
