using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;

namespace DiamondLuxurySolution.AdminCrewApp.Service.KnowledgeNews
{
    public class KnowledgeNewsApiService : BaseApiService, IKnowLedgeNewsApiService
    {
        public KnowledgeNewsApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateKnowledgeNews(CreateKnowledgeNewsRequest request)
        {
            var data = await PostAsyncHasImage<bool>("api/KnowLedgeNews/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteKnowledgeNews(DeleteKnowledgeNewsRequest request)
        {
            var data = await DeleteAsync<bool>($"api/KnowLedgeNews/Delete?KnowLedgeNewsId={request.KnowledgeNewsId}");
            return data;
        }


        public async Task<ApiResult<KnowledgeNewsVm>> GetKnowledgeNewsById(int KnowledgeNewsId)
        {
            var data = await GetAsync<KnowledgeNewsVm>($"api/KnowLedgeNews/GetById?KnowLedgeNewsId={KnowledgeNewsId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateKnowledgeNews(UpdateKnowledgeNewsRequest request)
        {
            var data = await PutAsyncHasImage<bool>("api/KnowLedgeNews/Update", request);
            return data;
        }

        public async Task<ApiResult<PageResult<KnowledgeNewsVm>>> ViewKnowledgeNews(ViewKnowledgeNewsRequest request)
        {
            var data = await GetAsync<PageResult<KnowledgeNewsVm>>($"api/KnowledgeNews/ViewKnowledgeNews?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
