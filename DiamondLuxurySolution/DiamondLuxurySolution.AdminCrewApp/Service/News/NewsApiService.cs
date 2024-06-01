using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.News;
using DiamondLuxurySolution.ViewModel.Models.Role;

namespace DiamondLuxurySolution.AdminCrewApp.Service.News
{
    public class NewsApiService : BaseApiService, INewsApiService
    {
        public NewsApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateNews(CreateNewsRequest request)
        {
            var data = await PostAsyncHasImage<bool>("api/News/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteNews(DeleteNewsRequest request)
        {
            var data = await DeleteAsync<bool>("api/News/Delete?NewsId="+ request.NewsId);
            return data;
        }

        public async Task<ApiResult<NewsVm>> GetNewsById(int NewsId)
        {
            var data = await GetAsync<NewsVm>("api/News/GetById?NewsId=" +NewsId);
            return data;
        }

        public async Task<ApiResult<bool>> UpdateNews(UpdateNewsRequest request)
        {
            var data = await PutAsyncHasImage<bool>("api/News/Update", request);
            return data;
        }

        public async Task<ApiResult<PageResult<NewsVm>>> ViewNews(ViewNewsRequest request)
        {
            var data = await GetAsync<PageResult<NewsVm>>($"api/News/View?Keyword={request.Keyword}&pageIndex={request.pageIndex}");

            return data;
        }
    }
}
