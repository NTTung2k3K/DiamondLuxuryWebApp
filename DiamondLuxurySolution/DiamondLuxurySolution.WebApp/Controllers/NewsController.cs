using DiamondLuxurySolution.WebApp.Service.News;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsApiService _NewsApiService;

        public NewsController(INewsApiService newsApiService)
        {
            _NewsApiService = newsApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            var result = await _NewsApiService.GetNewsById(id);
            if (result == null || !result.IsSuccessed)
            {
                return NotFound();
            }
            return View(result.ResultObj);
        }
    }
}

