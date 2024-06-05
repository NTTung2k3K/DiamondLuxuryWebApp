using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using DiamondLuxurySolution.ViewModel.Models.Warranty;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Warranty
{
	public class WarrantyApiService : BaseApiService, IWarrantyApiService
	{
		public WarrantyApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
		{
		}

		public async Task<ApiResult<bool>> CreateWarranty(CreateWarrantyRequest request)
		{
			var data = await PostAsync<bool>("api/Warrantys/Create", request);
			return data;
		}

		public async Task<ApiResult<bool>> DeleteWarranty(DeleteWarrantyRequest request)
		{
			var data = await DeleteAsync<bool>($"api/Warrantys/Delete?WarrantyId=" + request.WarrantyId);
			return data;
		}

		public async Task<ApiResult<List<WarrantyVm>>> GetAll()
		{
			var data = await GetAsync<List<WarrantyVm>>("api/Warrantys/GetAll");
			return data;
		}

		public async Task<ApiResult<WarrantyVm>> GetWarrantyById(Guid WarrantyId)
		{
			var data = await GetAsync<WarrantyVm>($"api/Warrantys/GetById?WarrantyId={WarrantyId}");
			return data;
		}

		public async Task<ApiResult<bool>> UpdateWarranty(UpdateWarrantyRequest request)
		{
			var data = await PutAsync<bool>("api/Warrantys/Update", request);
			return data;
		}

		public Task<ApiResult<PageResult<WarrantyVm>>> ViewWarrantyInCustomer(ViewWarrantyRequest request)
		{
			throw new NotImplementedException();
		}

		public async Task<ApiResult<PageResult<WarrantyVm>>> ViewWarrantyInManager(ViewWarrantyRequest request)
		{
			var data = await GetAsync<PageResult<WarrantyVm>>($"api/Warrantys/ViewInManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
			return data;
		}
	}
}
