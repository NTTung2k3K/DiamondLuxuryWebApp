using DiamondLuxurySolution.AdminCrewApp.Service.Platform;
using DiamondLuxurySolution.AdminCrewApp.Service.Slide;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

    public class SlideController : BaseController
    {
        private readonly ISlideApiService _SlideApiService;
        public SlideController(ISlideApiService slideApiService)
        {
            _SlideApiService = slideApiService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(ViewSlideRequest request)
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

                var platform = await _SlideApiService.ViewSlidesInManager(request);
                return View(platform.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int slideID)
        {
            try
            {
                var status = await _SlideApiService.GetSlideById(slideID);
                if (status is ApiErrorResult<SlideViewModel> errorResult)
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
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(status.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int slideId)
        {
            try
            {
                var platform = await _SlideApiService.GetSlideById(slideId);
                if (platform is ApiErrorResult<SlideViewModel> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (platform.Message != null)
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
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(platform.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateSlideRequest request)
        {
            try
            {
                var slide = await _SlideApiService.GetSlideById(request.SlideId);

                if (!ModelState.IsValid)
                {

                    SlideViewModel slideViewModel = new SlideViewModel()
                    {
                        SlideId = request.SlideId,
                        Description = request.Description,
                        SlideName = request.SlideName,
                        SlideUrl = request.SlideUrl,
                        Status = request.Status,
                        SlideImage = slide.ResultObj.SlideImage,
                    };
                    TempData["WarningToast"] = true;
                    return View(slideViewModel);
                }


                var status = await _SlideApiService.UpdateSlide(request);
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
                    TempData["WarningToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }
                TempData["SuccessToast"] = true;
                return RedirectToAction("Index", "Slide");
            }
            catch
            {
                TempData["WarningToast"] = true;
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int slideID)
        {
            try
            {
                var platform = await _SlideApiService.GetSlideById(slideID);
                if (platform is ApiErrorResult<SlideViewModel> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (platform.Message != null)
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
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(platform.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteSlideRequest request)
        {
            try
            {


                var status = await _SlideApiService.DeleteSlide(request);
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
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }
                TempData["SuccessToast"] = true;
                return RedirectToAction("Index", "Slide");

            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideRequest request)
        {



            var status = await _SlideApiService.CreateSlide(request);

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
                TempData["WarningToast"] = true;
                ViewBag.Errors = listError;
                return View();

            }
            TempData["SuccessToast"] = true;

            return RedirectToAction("Index", "Slide");
        }
    }
}
