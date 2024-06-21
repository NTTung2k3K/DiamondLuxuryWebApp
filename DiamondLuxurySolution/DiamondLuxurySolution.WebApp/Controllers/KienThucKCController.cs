using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.KnowledgeNews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class KienThucKCController : Controller
    {
        private readonly IKnowLedgeNewsApiService _knowledgeNewsApiService;

        public KienThucKCController(IKnowLedgeNewsApiService knowledgeNewsApiService)
        {
            _knowledgeNewsApiService = knowledgeNewsApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            var result = await _knowledgeNewsApiService.GetKnowledgeNewsById(id);
            if (result == null || !result.IsSuccessed)
            {
                return NotFound();
            }
            return View(result.ResultObj);
        }
    }
}
