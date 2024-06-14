using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.TrangSucKimCuong.TSKC_LacTay
{
    public class TSKC_LacTayViewComponent : ViewComponent
    {
        private readonly IProductApiService _productApiService;

        public TSKC_LacTayViewComponent(IProductApiService productApiService)
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
