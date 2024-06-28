using DiamondLuxurySolution.WebApp.Service.News;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.KnowledgeNews;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.WebApp.Repository.Components.News
{
    public class NewsViewComponent : ViewComponent
    {
        private readonly INewsApiService _NewsApiService;

        public NewsViewComponent(INewsApiService newsApiService)
        {
            _NewsApiService = newsApiService;
        }

        // Part Slide
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _NewsApiService.GetAll();
            return View(status.ResultObj.ToList());
        }
    }
}
