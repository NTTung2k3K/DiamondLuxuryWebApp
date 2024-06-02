using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;

namespace DiamondLuxurySolution.AdminCrewApp.Service.GemPriceList
{
    public class GemPriceListApiService : BaseApiService, IGemPriceListApiService
    {
        public GemPriceListApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateGemPriceList(CreateGemPriceListRequest request)
        {
            var data = await PostAsync<bool>("api/GemPriceList/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteGemPriceList(DeleteGemPriceListRequest request)
        {
            var data = await DeleteAsync<bool>($"api/GemPriceList/Delete?GemPriceListId={request.GemPriceListId}");
            return data;
        }

        public async Task<ApiResult<List<GemPriceListVm>>> GetAll()
        {
            var data = await GetAsync<List<GemPriceListVm>>("api/GemPriceList/GetAll");
            return data;
        }

        public async Task<ApiResult<GemPriceListVm>> GetGemPriceListById(int GemPriceListId)
        {
            var data = await GetAsync<GemPriceListVm>($"api/GemPriceList/GetById?GemPriceListId={GemPriceListId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateGemPriceList(UpdateGemPriceListRequest request)
        {
            var data = await PutAsync<bool>("api/GemPriceList/Update", request);
            return data;
        }

        public async Task<ApiResult<PageResult<GemPriceListVm>>> ViewGemPriceList(ViewGemPriceListRequest request)
        {
            var data = await GetAsync<PageResult<GemPriceListVm>>($"api/GemPriceList/ViewGemPriceList?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
