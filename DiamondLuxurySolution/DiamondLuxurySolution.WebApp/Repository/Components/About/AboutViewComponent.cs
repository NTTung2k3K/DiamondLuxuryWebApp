using DiamondLuxurySolution.WebApp.Service.About;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.About
{
    public class AboutViewComponent : ViewComponent
    {
        public readonly IAboutApiService _aboutApiService;
        public AboutViewComponent(IAboutApiService aboutApiService)
        {
            _aboutApiService = aboutApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _aboutApiService.GetAll();
            return View(data.ResultObj.ToList());
        }
    }
}
