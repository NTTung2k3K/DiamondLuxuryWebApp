using DiamondLuxurySolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.User.Staff
{
    public class ViewStaffPaginationCommonRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
    }
}
