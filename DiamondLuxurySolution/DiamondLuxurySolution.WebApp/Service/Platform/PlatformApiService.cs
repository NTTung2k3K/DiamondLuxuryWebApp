using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.WebApp.Services;

namespace DiamondLuxurySolution.WebApp.Service.Platform
{
    public class PlatformApiService : BaseApiService, IPlatformApiService
    {
        public PlatformApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<List<PlatfromVm>>> GetAll()
        {
            var data = await GetAsync<List<PlatfromVm>>("api/Platforms/GetAll");
            return data;
        }
    }
}
