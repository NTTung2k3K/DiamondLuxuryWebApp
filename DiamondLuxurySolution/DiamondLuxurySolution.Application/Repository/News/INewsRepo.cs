using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.News
{
    public interface INewsRepo
    {
        public Task<ApiResult<bool>> CreateNews(CreateNewsRequest request);
        public Task<ApiResult<bool>> UpdateNews(UpdateNewsRequest request);
        public Task<ApiResult<bool>> DeleteNews(DeleteNewsRequest request);
        public Task<ApiResult<NewsVm>> GetNewsById(int NewsId);
        public Task<ApiResult<PageResult<NewsVm>>> ViewNews(ViewNewsRequest request);


    }
}
