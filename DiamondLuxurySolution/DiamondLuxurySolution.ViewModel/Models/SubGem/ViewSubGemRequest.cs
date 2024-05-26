using DiamondLuxurySolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.SubGem
{
    public class ViewSubGemRequest : PagingRequestBase
    {
        public string? KeyWord { get; set; }
    }
}
