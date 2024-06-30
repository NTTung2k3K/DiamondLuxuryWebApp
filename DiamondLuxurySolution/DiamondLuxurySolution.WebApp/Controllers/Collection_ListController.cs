using DiamondLuxurySolution.WebApp.Service.Collection;
using DiamondLuxurySolution.WebApp.Service.Slide;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class Collection_ListController : Controller
    {
        private readonly ICollectionApiService _collectionApiService;
        public Collection_ListController(ICollectionApiService collectionApiService)
        {
            _collectionApiService = collectionApiService;
        }
        public async Task<IActionResult> Index(string collectionId)
        {

            var collection = await _collectionApiService.GetCollectionById(collectionId);
            if (collection != null)
            {
                return View(collection.ResultObj);
            }
            return View();
        }
    }
}
