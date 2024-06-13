using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.TrangSucKimCuong.TSKC_NhanNam
{
	public class TSKC_NhanNamViewComponent : ViewComponent
	{
		private readonly IProductApiService _productApiService;

		public TSKC_NhanNamViewComponent(IProductApiService productApiService)
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
