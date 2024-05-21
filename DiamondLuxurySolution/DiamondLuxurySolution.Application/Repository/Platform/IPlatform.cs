using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Platform
{
    public interface IPlatform
    {
        public Task<ApiResult<bool>> CreatePlatform(CreatePlatformRequest request);
    }
}
