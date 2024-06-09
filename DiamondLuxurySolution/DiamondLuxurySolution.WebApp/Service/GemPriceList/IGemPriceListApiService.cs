using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;

namespace DiamondLuxurySolution.WebApp.Service.GemPriceList
{
    public interface IGemPriceListApiService
    {
        public Task<ApiResult<List<GemPriceListVm>>> GetAll();
        public Task<ApiResult<bool>> CreateGemPriceList(CreateGemPriceListRequest request);
        public Task<ApiResult<bool>> UpdateGemPriceList(UpdateGemPriceListRequest request);
        public Task<ApiResult<bool>> DeleteGemPriceList(DeleteGemPriceListRequest request);
        public Task<ApiResult<GemPriceListVm>> GetGemPriceListById(int GemPriceListId);
        public Task<ApiResult<PageResult<GemPriceListVm>>> ViewGemPriceList(ViewGemPriceListRequest request);
    }
}
