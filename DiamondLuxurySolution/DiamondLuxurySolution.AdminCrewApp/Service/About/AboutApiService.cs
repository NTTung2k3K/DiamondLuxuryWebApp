using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Slide;

namespace DiamondLuxurySolution.AdminCrewApp.Service.About
{
    public class AboutApiService : BaseApiService, IAboutApiService
    {
        public AboutApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<bool>> CreateAbout(CreateAboutRequest request)
        {
            var data = await PostAsync<bool>("api/Abouts/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteAbout(DeleteAboutRequest request)
        {
            var data = await DeleteAsync<bool>($"api/Abouts/Delete?aboutId={request.AboutId}");
            return data;
        }

        public async Task<ApiResult<AboutVm>> GetAboutById(int AboutId)
        {
            var data = await GetAsync<AboutVm>($"api/Abouts/GetById?aboutId={AboutId}");
            return data;
        }

        public async Task<ApiResult<List<AboutVm>>> GetAll()
        {
            var data = await GetAsync<List<AboutVm>>("api/Abouts/GetAll");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateAbout(UpdateAboutRequest request)
        {
            var data = await PutAsync<bool>("api/Abouts/Update", request);
            return data;
        }

        public Task<ApiResult<PageResult<AboutVm>>> ViewAboutInCustomer(ViewAboutRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<AboutVm>>> ViewAboutInManager(ViewAboutRequest request)
        {
            var data = await GetAsync<PageResult<AboutVm>>($"api/Abouts/ViewInManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
