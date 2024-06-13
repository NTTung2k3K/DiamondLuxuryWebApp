using DiamondLuxurySolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Product
{
    public class ViewProductRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
	}
}
