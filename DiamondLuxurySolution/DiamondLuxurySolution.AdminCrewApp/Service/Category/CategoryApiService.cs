using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Service.Category;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Category;
using System;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Category
{
	public class CategoryApiService : BaseApiService, ICategoryApiService
	{
		public CategoryApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
		{
		}
		public async Task<ApiResult<bool>> CreateCategory(CreateCategoryRequest request)
		{
			var data = await PostAsyncHasImage<bool>("api/Categories/Create", request);
			return data;
		}

		public async Task<ApiResult<bool>> DeleteCategory(DeleteCategoryRequest request)
		{
			var data = await DeleteAsync<bool>($"api/Categories/Delete?categoryId={request.CategoryId}");
			return data;
		}

		public async Task<ApiResult<List<CategoryVm>>> GetAll()
		{
			var data = await GetAsync<List<CategoryVm>>("api/Categories/GetAll");
			return data;
		}

		public async Task<ApiResult<CategoryVm>> GetCategoryById(int CategoryId)
		{
			var data = await GetAsync<CategoryVm>($"api/Categories/GetById?CategoryId={CategoryId}");
			return data;
		}

		public async Task<ApiResult<bool>> UpdateCategory(UpdateCategoryRequest request)
		{
			var data = await PutAsyncHasImage<bool>("api/Categories/Update", request);
			return data;
		}

		public Task<ApiResult<PageResult<CategoryVm>>> ViewCategoryInCustomer(ViewCategoryRequest request)
		{
			throw new NotImplementedException();
		}

		public async Task<ApiResult<PageResult<CategoryVm>>> ViewCategoryInManager(ViewCategoryRequest request)
		{
			var data = await GetAsync<PageResult<CategoryVm>>($"api/Categories/ViewInManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
			return data;
		}
	}
}
