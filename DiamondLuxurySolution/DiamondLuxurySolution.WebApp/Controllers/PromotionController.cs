using DiamondLuxurySolution.WebApp.Service.Promotion;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Promotion
{
    public class PromotionController : Controller
    {
        private readonly IPromotionApiService _PromotionApiService;

        public PromotionController(IPromotionApiService promotionApiService)
        {
			_PromotionApiService = promotionApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var result = await _PromotionApiService.GetPromotionById(id);
            if (result == null || !result.IsSuccessed)
            {
                return NotFound();
            }
            return View(result.ResultObj);
        }
    }
}

