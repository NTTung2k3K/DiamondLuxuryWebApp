using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.Diamond.Diamond
{
	public class DiamondViewComponent : ViewComponent
	{
		private readonly IProductApiService _productApiService;

		public DiamondViewComponent(IProductApiService productApiService)
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
