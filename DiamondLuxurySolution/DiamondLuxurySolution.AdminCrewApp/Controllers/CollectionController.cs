using DiamondLuxurySolution.AdminCrewApp.Service.Collection;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Collection;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ICollectionApiService _collectionApiService;

        public CollectionController(ICollectionApiService collectionApiService)
        {
            _collectionApiService = collectionApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewCollectionRequest request)
        {
            try
            {

                ViewBag.txtLastSeachValue = request.Keyword;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (TempData["FailMsg"] != null)
                {
                    ViewBag.FailMsg = TempData["FailMsg"];
                }
                if (TempData["SuccessMsg"] != null)
                {
                    ViewBag.SuccessMsg = TempData["SuccessMsg"];
                }

                var collection = await _collectionApiService.ViewCollectionInPaging(request);
                return View(collection.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string CollectionId)
        {
            try
            {
                var status = await _collectionApiService.GetCollectionById(CollectionId);
                if (status is ApiErrorResult<CollectionVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (status.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    else if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in listError)
                        {
                            listError.Add(error);
                        }
                    }
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(status.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string CollectionId)
        {
            try
            {
                var collection = await _collectionApiService.GetCollectionById(CollectionId);
                if (collection is ApiErrorResult<CollectionVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in listError)
                        {
                            listError.Add(error);
                        }
                    }
                    else if (collection.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(collection.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCollectionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    CollectionVm collectionVm = new CollectionVm()
                    {
                        CollectionId = request.CollectionId,
                        Description = request.Description,
                        Thumbnail = request.Thumbnail.ToString(),
                        Status = request.Status,
                    };
                    return View(collectionVm);
                }
                var status = await _collectionApiService.UpdateCollection(request);
                if (status is ApiErrorResult<bool> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in listError)
                        {
                            listError.Add(error);
                        }
                    }
                    else if (status.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    ViewBag.Errors = listError;
                    return View();

                }
                return RedirectToAction("Index", "Collection");
            }
            catch
            {
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string CollectionId)
        {
            try
            {
                var collection = await _collectionApiService.GetCollectionById(CollectionId);
                if (collection is ApiErrorResult<CollectionVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (collection.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    else if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in listError)
                        {
                            listError.Add(error);
                        }
                    }
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(collection.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCollectionRequest request)
        {
            try
            {
                var status = await _collectionApiService.DeleteCollection(request);
                if (status is ApiErrorResult<bool> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (status.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    else if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in listError)
                        {
                            listError.Add(error);
                        }
                    }
                    ViewBag.Errors = listError;
                    return View();

                }
                return RedirectToAction("Index", "Collection");

            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCollectionRequest request)
        {


            var status = await _collectionApiService.CreateCollection(request);

            if (status is ApiErrorResult<bool> errorResult)
            {
                List<string> listError = new List<string>();

                if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                {
                    foreach (var error in errorResult.ValidationErrors)
                    {
                        listError.Add(error);
                    }
                }
                else if (status.Message != null)
                {
                    listError.Add(errorResult.Message);
                }
                ViewBag.Errors = listError;
                return View();

            }
            TempData["SuccessMsg"] = "Create success for Role " + request.CollectionName;

            return RedirectToAction("Index", "Collection");
        }


    }
}

