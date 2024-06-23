using DiamondLuxurySolution.AdminCrewApp.Service.Category;
using DiamondLuxurySolution.Application.Repository.Category;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.KnowledgeNews;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.WebApp.Repository.Components.Home_Category
{
    public class Home_CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryApiService _categoryApiService;

        public Home_CategoryViewComponent(ICategoryApiService categoryApiService)
        {
            _categoryApiService = categoryApiService;
        }

        // Part Slide
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _categoryApiService.GetAll();
            return View(status.ResultObj.ToList());
        }
    }
}
