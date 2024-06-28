using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.ViewModel.Models.News;

namespace DiamondLuxurySolution.WebApp.Service.News
{
    public interface INewsApiService
    {
        public Task<ApiResult<List<NewsVm>>> GetAll();
        public Task<ApiResult<bool>> CreateNews(CreateNewsRequest request);
        public Task<ApiResult<bool>> UpdateNews(UpdateNewsRequest request);
        public Task<ApiResult<bool>> DeleteNews(DeleteNewsRequest request);
        public Task<ApiResult<NewsVm>> GetNewsById(int NewsId);
        public Task<ApiResult<PageResult<NewsVm>>> ViewNews(ViewNewsRequest request);

    }
}
