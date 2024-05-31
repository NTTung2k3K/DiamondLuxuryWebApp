using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Gem;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Gem
{
    public class GemApiService : BaseApiService, IGemApiService
    {
        public GemApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<bool>> CreateGem(CreateGemRequest request)
        {
            var data = await PostAsyncHasImage<bool>("api/Gems/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteGem(DeleteGemRequest request)
        {
            var data = await DeleteAsync<bool>($"api/Gems/Delete?GemId={request.GemId}");
            return data;
        }

        public async Task<ApiResult<List<GemVm>>> GetAll()
        {
            var data = await GetAsync<List<GemVm>>("api/Gems/GetAll");
            return data;
        }

        public async Task<ApiResult<GemVm>> GetGemById(Guid GemId)
        {
            var data = await GetAsync<GemVm>($"api/Gems/GetById?GemId={GemId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateGem(UpdateGemResquest request)
        {
            var data = await PutAsyncHasImage<bool>("api/Gems/Update", request);
            return data;
        }

        public Task<ApiResult<PageResult<GemVm>>> ViewGemInCustomer(ViewGemRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<GemVm>>> ViewGemInManager(ViewGemRequest request)
        {
            var data = await GetAsync<PageResult<GemVm>>($"api/Gems/ViewManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
