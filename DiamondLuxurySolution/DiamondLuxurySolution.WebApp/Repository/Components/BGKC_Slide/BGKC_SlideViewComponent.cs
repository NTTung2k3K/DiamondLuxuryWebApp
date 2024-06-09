using DiamondLuxurySolution.WebApp.Service.Slide;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.BGKC_Slide
{
    public class BGKC_SlideViewComponent : ViewComponent
    {
        private readonly ISlideApiService _slideApiService;

        public BGKC_SlideViewComponent(ISlideApiService slideApiService)
        {
            _slideApiService = slideApiService;
        }
        //Part Slide
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _slideApiService.GetAll();
            return View(status.ResultObj.ToList());
        }
    }
}
