using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.TrangSucCuoi.TSC_ProposalRing
{
    public class TSC_ProposalRingViewComponent : ViewComponent
    {
        private readonly IProductApiService _productApiService;

        public TSC_ProposalRingViewComponent(IProductApiService productApiService)
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
