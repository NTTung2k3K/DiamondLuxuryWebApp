using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Slide
{
    public interface ISlideRepo
    {
        public Task<ApiResult<bool>> CreateSlide(CreateSlideRequest request);
        public Task<ApiResult<bool>> UpdateSlide(UpdateSlideRequest request);
        public Task<ApiResult<bool>> DeleteSlide(DeleteSlideRequest request);
        public Task<ApiResult<SlideViewModel>> GetSlideById(int SlideId);
        public Task<ApiResult<PageResult<SlideViewModel>>> ViewSlidesInCustomer(ViewSlideRequest request);
        public Task<ApiResult<PageResult<SlideViewModel>>> ViewSlidesInManager(ViewSlideRequest request);
    }
}
