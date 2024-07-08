using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.KnowledgeNews;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.WebApp.Repository.Components.KienThucTS
{
    public class KnowledgeJewelryViewComponent : ViewComponent
    {
        private readonly IKnowLedgeNewsApiService _knowledgeNewsApiService;

        public KnowledgeJewelryViewComponent(IKnowLedgeNewsApiService knowledgeNewsApiService)
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
