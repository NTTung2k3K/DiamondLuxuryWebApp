using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.Diamond.DiamondNatural
{
	public class DiamondNaturalViewComponent : ViewComponent
	{
		private readonly IProductApiService _productApiService;

		public DiamondNaturalViewComponent(IProductApiService productApiService)
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
