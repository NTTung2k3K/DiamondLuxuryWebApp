using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.WebApp.Service.Product;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchProductApiService _searchProductApiService;

        public SearchController(ISearchProductApiService searchProductApiService)
        {
            _searchProductApiService = searchProductApiService;
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

                var apiResult = await _searchProductApiService.GetAll();
                if (apiResult == null || !apiResult.IsSuccessed || apiResult.ResultObj == null)
                {
                    ViewBag.Message = "Không tìm thấy kết quả phù hợp";
                    return View();
                }

                var results = new PageResult<ProductVm>();
                var filteredProducts = new List<ProductVm>();

                if (apiResult != null && apiResult.IsSuccessed && apiResult.ResultObj != null)
                {
                    filteredProducts = apiResult.ResultObj;

                    if (!string.IsNullOrEmpty(request.Keyword))
                    {
                        var keyword = request.Keyword.ToLower();
                        filteredProducts = filteredProducts
                            .Where(p => p.ProductName.ToLower().Contains(keyword) ||
                                        (p.CategoryVm?.CategoryName?.ToLower().Contains(keyword) ?? false))
                            .ToList();
                    }

                    results.Items = filteredProducts;
                    results.TotalRecords = filteredProducts.Count;
                }
                else
                {
                    results.Items = new List<ProductVm>(); // Hoặc gán null tùy vào logic của bạn
                    results.TotalRecords = 0;
                }

                ViewData["Keyword"] = request.Keyword;
                ViewData["SearchResultCount"] = results.TotalRecords;
                return View(results);
            }
            catch
            {
                return View();
            }
        }
    }
}
