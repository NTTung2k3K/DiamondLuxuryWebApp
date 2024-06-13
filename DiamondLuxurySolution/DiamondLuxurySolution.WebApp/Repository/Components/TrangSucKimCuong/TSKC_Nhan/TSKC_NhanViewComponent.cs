using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.TrangSucKimCuong.TSKC_Nhan
{
    public class TSKC_NhanViewComponent : ViewComponent
    {
        private readonly IProductApiService _productApiService;

        public TSKC_NhanViewComponent(IProductApiService productApiService)
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
