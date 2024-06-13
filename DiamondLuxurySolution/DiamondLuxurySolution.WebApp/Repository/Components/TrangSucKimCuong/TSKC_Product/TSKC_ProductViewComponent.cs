using Azure.Core;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.TrangSucKimCuong.TSKC_Product
{
    public class TSKC_ProductViewComponent : ViewComponent
    {

        private readonly IProductApiService _productApiService;

        public TSKC_ProductViewComponent(IProductApiService productApiService)
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
