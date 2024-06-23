using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.KnowledgeNews;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.WebApp.Repository.Components.KienThucKC
{
    public class KienThucKCViewComponent : ViewComponent
    {
        private readonly IKnowLedgeNewsApiService _knowledgeNewsApiService;

        public KienThucKCViewComponent(IKnowLedgeNewsApiService knowledgeNewsApiService)
        {
            _knowledgeNewsApiService = knowledgeNewsApiService;
        }

        // Part Slide
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _knowledgeNewsApiService.GetAll();
            return View(status.ResultObj.ToList());
        }
    }
}
