using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.TrangSucKimCuong.TSKC_NhanNu
{
	public class TSKC_NhanNuViewComponent : ViewComponent
	{
		private readonly IProductApiService _productApiService;

		public TSKC_NhanNuViewComponent(IProductApiService productApiService)
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
