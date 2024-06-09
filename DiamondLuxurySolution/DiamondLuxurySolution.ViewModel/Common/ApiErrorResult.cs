using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public List<string> ValidationErrors { get; set; }

        public ApiErrorResult() 
        {
            IsSuccessed = false;
        }
        public ApiErrorResult(string message)
        {
            Message = message;
            IsSuccessed = false;
        }

        public ApiErrorResult(string message, List<string> validationErrors)
        {
                ValidationErrors = validationErrors;
            Message = message;
            IsSuccessed = false;
        }
    }
}
