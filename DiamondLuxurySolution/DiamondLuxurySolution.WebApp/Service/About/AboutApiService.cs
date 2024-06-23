using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.WebApp.Services;

namespace DiamondLuxurySolution.WebApp.Service.About
{
    public class AboutApiService : BaseApiService, IAboutApiService
    {
        public AboutApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<List<AboutVm>>> GetAll()
        {
            var data = await GetAsync<List<AboutVm>>("api/Abouts/GetAll");
            return data;
        }
    }
}
