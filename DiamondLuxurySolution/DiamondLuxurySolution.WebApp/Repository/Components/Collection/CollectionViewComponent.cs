using DiamondLuxurySolution.WebApp.Service.Collection;
using DiamondLuxurySolution.WebApp.Service.Slide;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Repository.Components.Collection
{
    public class CollectionViewComponent : ViewComponent
    {
        private readonly ICollectionApiService _collectionApiService;
		private readonly ISlideApiService _slideApiService;

		public CollectionViewComponent(ICollectionApiService collectionApiService, ISlideApiService slideApiService)
        {
            _collectionApiService = collectionApiService;
			_slideApiService = slideApiService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var status = await _collectionApiService.GetAll();
            var slideList = await _slideApiService.GetAll();
            ViewBag.ListSlide = slideList.ResultObj.ToList();
            return View(status.ResultObj.ToList());
        }
    }
}
