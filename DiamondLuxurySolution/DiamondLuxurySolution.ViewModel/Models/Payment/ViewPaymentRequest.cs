using DiamondLuxurySolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Payment
{
    public class ViewPaymentRequest : PagingRequestBase
    {
        public string? KeyWord {  get; set; }
    }
}
