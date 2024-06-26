using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.WebApp.Service.Collection;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class Collection_MainController : Controller
    {
        private readonly ICollectionApiService _collectionApiService;

        public Collection_MainController(ICollectionApiService collectionApiService)
        {
            _collectionApiService = collectionApiService;
        }
        public async Task<IActionResult> Index()
        {
            var status = await _collectionApiService.GetAll();
            return View(status.ResultObj.ToList());
        }
    }
}
