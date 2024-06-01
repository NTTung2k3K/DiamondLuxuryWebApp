using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.News;

namespace DiamondLuxurySolution.AdminCrewApp.Service.News
{
    public interface INewsApiService
    {
        public Task<ApiResult<bool>> CreateNews(CreateNewsRequest request);
        public Task<ApiResult<bool>> UpdateNews(UpdateNewsRequest request);
        public Task<ApiResult<bool>> DeleteNews(DeleteNewsRequest request);
        public Task<ApiResult<NewsVm>> GetNewsById(int NewsId);
        public Task<ApiResult<PageResult<NewsVm>>> ViewNews(ViewNewsRequest request);

    }
}
