using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Category;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Category
{
	public interface ICategoryApiService
	{
		public Task<ApiResult<List<CategoryVm>>> GetAll();
		public Task<ApiResult<bool>> CreateCategory(CreateCategoryRequest request);
		public Task<ApiResult<bool>> UpdateCategory(UpdateCategoryRequest request);
		public Task<ApiResult<bool>> DeleteCategory(DeleteCategoryRequest request);
		public Task<ApiResult<CategoryVm>> GetCategoryById(int CategoryId);
		public Task<ApiResult<PageResult<CategoryVm>>> ViewCategoryInCustomer(ViewCategoryRequest request);

		public Task<ApiResult<PageResult<CategoryVm>>> ViewCategoryInManager(ViewCategoryRequest request);
	}
}
