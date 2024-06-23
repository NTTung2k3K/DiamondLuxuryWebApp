using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.Product;
using DiamondLuxurySolution.WebApp.Service.Slide;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.WebApp.Repository.Components.Home_BongTai
{
    public class Home_BongTaiViewComponent : ViewComponent
    {
        private readonly IProductApiService _productApiService;
        private readonly ISlideApiService _slideApiService;

        public Home_BongTaiViewComponent(IProductApiService productApiService, ISlideApiService slideApiService)
        {
            _productApiService = productApiService;
            _slideApiService = slideApiService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _productApiService.GetAll();
            var listSlide = await _slideApiService.GetAll();
            ViewBag.listSlide = listSlide.ResultObj.ToList();
            return View(status.ResultObj.ToList());
        }
    }
}
