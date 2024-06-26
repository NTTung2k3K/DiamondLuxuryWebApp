using DiamondLuxurySolution.WebApp.Service.News;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.KnowledgeNews;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using DiamondLuxurySolution.WebApp.Service.Promotion;

namespace DiamondLuxurySolution.WebApp.Repository.Components.Promotion
{
    public class PromotionViewComponent : ViewComponent
    {
        private readonly IPromotionApiService _pomotionApiService;

        public PromotionViewComponent(IPromotionApiService pomotionApiService)
        {
            _pomotionApiService = pomotionApiService;
        }

        // Part Slide
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _pomotionApiService.GetAll();
            return View(status.ResultObj.ToList());
        }
    }
}
