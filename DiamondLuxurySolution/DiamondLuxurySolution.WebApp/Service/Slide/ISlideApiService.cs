using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Slide;

namespace DiamondLuxurySolution.WebApp.Service.Slide
{
    public interface ISlideApiService
    {
        public Task<ApiResult<List<SlideViewModel>>> GetAll();
        public Task<ApiResult<bool>> CreateSlide(CreateSlideRequest request);
        public Task<ApiResult<bool>> UpdateSlide(UpdateSlideRequest request);
        public Task<ApiResult<bool>> DeleteSlide(DeleteSlideRequest request);
        public Task<ApiResult<SlideViewModel>> GetSlideById(int SlideId);
        public Task<ApiResult<PageResult<SlideViewModel>>> ViewSlidesInCustomer(ViewSlideRequest request);
        public Task<ApiResult<PageResult<SlideViewModel>>> ViewSlidesInManager(ViewSlideRequest request);
    }
}
