using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;

namespace DiamondLuxurySolution.WebApp.Service.Platform
{
    public interface IPlatformApiService
    {
        public Task<ApiResult<List<PlatfromVm>>> GetAll();

    }
}
