using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.WarrantyDetail;

namespace DiamondLuxurySolution.AdminCrewApp.Service.WarrantyDetail
{
    public interface IWarrantyDetailService
    {
        public Task<ApiResult<List<WarrantyDetailVm>>> GetAll();
        public Task<ApiResult<bool>> CreateWarrantyDetail(CreateWarrantyDetailRequest request);
        public Task<ApiResult<string>> CheckValidWarrantyId(string WarrantyId);
        public Task<ApiResult<bool>> UpdateWarrantyDetail(UpdateWarrantyDetailRequest request);
        public Task<ApiResult<bool>> DeleteWarrantyDetail(DeleteWarrantyDetailRequest request);
        public Task<ApiResult<WarrantyDetailVm>> GetWarrantyDetaiById(int WarrantyDetailId);
        public Task<ApiResult<PageResult<WarrantyDetailVm>>> ViewWarrantyDetai(ViewWarrantyDetailRequest request);
    }
}
