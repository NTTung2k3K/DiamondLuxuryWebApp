using DiamondLuxurySolution.WebApp.Service.GemPriceList;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.BGKC_GemPriceList
{
    public class BGKC_GemPriceListViewComponent : ViewComponent
    {
        private readonly IGemPriceListApiService _gemPriceListApiService;
        public BGKC_GemPriceListViewComponent(IGemPriceListApiService gemPriceListApiService)
        {
            _gemPriceListApiService = gemPriceListApiService;
        }
        //Part Slide
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _gemPriceListApiService.GetAll();
            return View(status.ResultObj.ToList());
        }
    }
}
