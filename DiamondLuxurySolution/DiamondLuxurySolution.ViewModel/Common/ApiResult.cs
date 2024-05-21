using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Common
{
    public class ApiResult<T>
    {
        public string Message { get; set; }
        public bool IsSuccessed { get; set; }
        public T ResultObj { get; set; }
    }
}
