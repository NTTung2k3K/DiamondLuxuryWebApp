using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchProductApiService _SearchProductApiService;

        public SearchController(ISearchProductApiService searchProductApiService)
        {
            _SearchProductApiService = searchProductApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewProductRequest request)
        {
            try
            {
                ViewBag.txtLastSearchValue = request.Keyword;

                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (TempData["FailMsg"] != null)
                {
                    ViewBag.FailMsg = TempData["FailMsg"];
                }

                if (TempData["SuccessMsg"] != null)
                {
                    ViewBag.SuccessMsg = TempData["SuccessMsg"];
                }

                var apiResult = await _SearchProductApiService.ViewProduct(request);
                if (apiResult == null || !apiResult.IsSuccessed)
                {
                    ViewBag.Message = "Không tìm thấy kết quả phù hợp";
                    return View();
                }

                var results = apiResult.ResultObj;
                ViewData["Keyword"] = request.Keyword;
                return View(results);
            }
            catch
            {
                return View();
            }
        }


    }
}
