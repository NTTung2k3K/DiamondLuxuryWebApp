using DiamondLuxurySolution.AdminCrewApp.Service.KnowledgeNewsCategoty;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory;
using DiamondLuxurySolution.ViewModel.Models.Slide;

namespace DiamondLuxurySolution.AdminCrewApp.Service.KnowledgeNewsCategory
{
    public class KnowledgeNewsCategoryApiService : BaseApiService, IKnowledgeNewsCategoryApiService
    {
        public KnowledgeNewsCategoryApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<bool>> CreateKnowledgeNewsCategory(CreateKnowledgeNewsCategoryRequest request)
        {
            var data = await PostAsync<bool>("api/KnowledgeNewsCategories/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteKnowledgeNewsCategory(DeleteKnowledgeNewsCategoryRequest request)
        {
            var data = await DeleteAsync<bool>($"api/KnowledgeNewsCategories/Delete?KnowledgeNewsCategoryId={request.KnowledgeNewsCategoryId}");
            return data;
        }

        public async Task<ApiResult<List<KnowledgeNewsCategoryVm>>> GetAll()
        {
            var data = await GetAsync<List<KnowledgeNewsCategoryVm>>("api/KnowledgeNewsCategories/GetAll");
            return data;
        }

        public async Task<ApiResult<KnowledgeNewsCategoryVm>> GetKnowledgeNewsCategoryById(int KnowledgeNewsCategoryId)
        {
            var data = await GetAsync<KnowledgeNewsCategoryVm>($"api/KnowledgeNewsCategories/GetById?KnowledgeNewCatagoryId={KnowledgeNewsCategoryId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateKnowledgeNewsCategory(UpdateKnowledgeNewsCategoryRequest request)
        {
            var data = await PutAsync<bool>("api/KnowledgeNewsCategories/Update", request);
            return data;
        }

        public async Task<ApiResult<PageResult<KnowledgeNewsCategoryVm>>> ViewKnowledgeNewsCategory(ViewKnowledgeNewsCategoryRequest request)
        {
            var data = await GetAsync<PageResult<KnowledgeNewsCategoryVm>>($"api/KnowledgeNewsCategories/ViewInCustomer?KeyWord={request.KeyWord}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
