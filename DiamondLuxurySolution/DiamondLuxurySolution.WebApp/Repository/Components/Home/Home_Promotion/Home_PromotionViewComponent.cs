using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.News;
using DiamondLuxurySolution.WebApp.Service.Product;
using DiamondLuxurySolution.WebApp.Service.Promotion;
using DiamondLuxurySolution.WebApp.Service.Slide;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.WebApp.Repository.Components.Home_Promotion
{
    public class Home_PromotionViewComponent : ViewComponent
    {
        private readonly IPromotionApiService _promotionApiService;

        public Home_PromotionViewComponent(IPromotionApiService promotionApiService)
        {
            _promotionApiService = promotionApiService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _promotionApiService.GetAll();
            return View(status.ResultObj.ToList());
        }
    }
}
