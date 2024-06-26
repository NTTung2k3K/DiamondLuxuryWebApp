using DiamondLuxurySolution.WebApp.Service.Collection;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.Collection_List
{
    public class Collection_ListViewComponent : ViewComponent
    {
        private readonly ICollectionApiService _collectionApiService;

        public Collection_ListViewComponent(ICollectionApiService collectionApiService)
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
