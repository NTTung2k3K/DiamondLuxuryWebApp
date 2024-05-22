using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Platform
{
    public interface IPlatformRepo
    {
        public Task<ApiResult<bool>> CreatePlatform(CreatePlatformRequest request);
        public Task<ApiResult<bool>> UpdatePlatform(UpdatePlatformRequest request);
        public Task<ApiResult<bool>> DeletePlatform(DeletePlatformRequest request);
        public Task<ApiResult<PlatfromVm>> GetPlatfromById(int PlatformId);
        public Task<ApiResult<PageResult<PlatfromVm>>> ViewPlatfrom(ViewPlatformRequest request);

    }
}
