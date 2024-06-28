using DiamondLuxurySolution.WebApp.Service.Platform;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.Platform
{
    public class PlatformViewComponent : ViewComponent
    {
        public readonly IPlatformApiService _platformApiService;
        public PlatformViewComponent(IPlatformApiService platformApiService)
        {
            _platformApiService = platformApiService;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _platformApiService.GetAll();
            return View(data.ResultObj.ToList());
        }
    }
}
