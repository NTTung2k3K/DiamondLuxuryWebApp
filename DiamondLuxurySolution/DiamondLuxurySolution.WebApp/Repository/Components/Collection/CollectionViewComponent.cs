using DiamondLuxurySolution.WebApp.Service.Collection;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.Collection
{
    public class CollectionViewComponent : ViewComponent
    {
        private readonly ICollectionApiService _collectionApiService;

        public CollectionViewComponent(ICollectionApiService collectionApiService)
        {
            _collectionApiService = collectionApiService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _collectionApiService.GetAll();
            return View(status.ResultObj.ToList());
        }
    }
}
