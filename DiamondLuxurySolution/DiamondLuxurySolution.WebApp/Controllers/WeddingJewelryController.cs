using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
	public class WeddingJewelryController : Controller
	{
		private readonly IProductApiService _productApiService;

		public WeddingJewelryController(IProductApiService productApiService)
		{
			_productApiService = productApiService;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Detail(string ProductId)
		{
			var result = await _productApiService.GetProductById(ProductId);
			if (result == null || !result.IsSuccessed)
			{
				return NotFound();
			}
			return View(result.ResultObj);
		}
	}
}
