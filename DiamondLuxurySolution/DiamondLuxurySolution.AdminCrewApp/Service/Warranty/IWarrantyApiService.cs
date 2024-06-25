using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Warranty;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Warranty
{
	public interface IWarrantyApiService
	{
		public Task<ApiResult<List<WarrantyVm>>> GetAll();
		public Task<ApiResult<bool>> CreateWarranty(CreateWarrantyRequest request);
		public Task<ApiResult<bool>> UpdateWarranty(UpdateWarrantyRequest request);
		public Task<ApiResult<bool>> DeleteWarranty(DeleteWarrantyRequest request);
		public Task<ApiResult<WarrantyVm>> GetWarrantyById(string WarrantyId);
		public Task<ApiResult<PageResult<WarrantyVm>>> ViewWarrantyInCustomer(ViewWarrantyRequest request);

		public Task<ApiResult<PageResult<WarrantyVm>>> ViewWarrantyInManager(ViewWarrantyRequest request);

	}
}
