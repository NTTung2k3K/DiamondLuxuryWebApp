using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.TrangSucCuoi.TSC_WeddingRing
{
    public class TSC_WeddingRingViewComponent : ViewComponent
    {
        private readonly IProductApiService _productApiService;

        public TSC_WeddingRingViewComponent(IProductApiService productApiService)
        {
            _productApiService = productApiService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _productApiService.GetAll();
            return View(status.ResultObj.ToList());
        }
    }
}
