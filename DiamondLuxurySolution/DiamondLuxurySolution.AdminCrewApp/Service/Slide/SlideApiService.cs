using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using System;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Slide
{
    public class SlideApiService: BaseApiService, ISlideApiService
    {
        public SlideApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateSlide(CreateSlideRequest request)
        {
            var data = await PostAsyncHasImage<bool>("api/Slides/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteSlide(DeleteSlideRequest request)
        {
            var data = await DeleteAsync<bool>($"api/Slides/Delete?slideID={request.SlideId}");
            return data;
        }

        public async Task<ApiResult<List<SlideViewModel>>> GetAll()
        {
            var data = await GetAsync<List<SlideViewModel>>("api/Slides/GetAll");
            return data;
        }

        public async Task<ApiResult<SlideViewModel>> GetSlideById(int SlideId)
        {
            var data = await GetAsync<SlideViewModel>($"api/Slides/GetById?slideID={SlideId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateSlide(UpdateSlideRequest request)
        {
            var data = await PutAsyncHasImage<bool>("api/Slides/Update", request);
            return data;
        }

        public Task<ApiResult<PageResult<SlideViewModel>>> ViewSlidesInCustomer(ViewSlideRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<SlideViewModel>>> ViewSlidesInManager(ViewSlideRequest request)
        {
            var data = await GetAsync<PageResult<SlideViewModel>>($"api/Slides/ViewInManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
