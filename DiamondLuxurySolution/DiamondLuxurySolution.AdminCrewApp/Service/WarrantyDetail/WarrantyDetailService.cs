using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Warranty;
using DiamondLuxurySolution.ViewModel.Models.WarrantyDetail;

namespace DiamondLuxurySolution.AdminCrewApp.Service.WarrantyDetail
{
    public class WarrantyDetailService : BaseApiService, IWarrantyDetailService
    {
        public async Task<ApiResult<bool>> CheckValidWarrantyId(string WarrantyId)
        {
            var data = await GetAsync<bool>("api/WarrantyDetails/ValidateWarrantyId"+WarrantyId);
            return data;
        }

        public async Task<ApiResult<bool>> CreateWarrantyDetail(CreateWarrantyDetailRequest request)
        {
            var data = await PostAsync<bool>("api/WarrantyDetails/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteWarrantyDetail(DeleteWarrantyDetailRequest request)
        {
            var data = await DeleteAsync<bool>("api/WarrantyDetails/Delete?WarrantyDetailId=" + request.WarrantyDetailId);
            return data;
        }

        public async Task<ApiResult<List<WarrantyDetailVm>>> GetAll()
        {
            var data = await GetAsync<List<WarrantyDetailVm>>("api/WarrantyDetails/GetAll");
            return data;
        }

        public async Task<ApiResult<WarrantyDetailVm>> GetWarrantyDetaiById(int WarrantyDetailId)
        {
            var data = await GetAsync<WarrantyDetailVm>("api/WarrantyDetails/GetById?WarrantyDetailId=" + WarrantyId);
            return data;
        }

        public async Task<ApiResult<bool>> UpdateWarrantyDetail(UpdateWarrantyDetailRequest request)
        {
            var data = await PutAsync<bool>("api/WarrantyDetails/Update", request);
            return data;
        }

        public  async Task<ApiResult<PageResult<WarrantyDetailVm>>> ViewWarrantyDetai(ViewWarrantyDetailRequest request)
        {
            var data = await GetAsync<PageResult<WarrantyDetailVm>>($"api/WarrantyDetails/ViewInManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
