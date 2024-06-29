using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Service.Platform;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Discount;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;
namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

    public class PlatformController : BaseController
    {
        private readonly IPlatformApiService _platformApiService;

        public PlatformController(IPlatformApiService platfromApiService)
        {
            _platformApiService = platfromApiService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(ViewPlatformRequest request)
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

                var platform = await _platformApiService.ViewPlatfromInManager(request);
                return View(platform.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int PlatformId)
        {
            try
            {
                var status = await _platformApiService.GetPlatfromById(PlatformId);
                if (status is ApiErrorResult<PlatfromVm> errorResult)
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
        public async Task<IActionResult> Edit(int PlatformId)
        {
            try
            {
                var platform = await _platformApiService.GetPlatfromById(PlatformId);
                if (platform is ApiErrorResult<PlatfromVm> errorResult)
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
                    TempData["WarningToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(platform.ResultObj);
            }
            catch
            {
                TempData["WarningToast"] = true;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdatePlatformRequest request)
        {
            try
            {
                var platform = await _platformApiService.GetPlatfromById(request.PlatformId);

                if (!ModelState.IsValid)
                {
                    PlatfromVm platfromVm = new PlatfromVm()
                    {
                        PlatformId = request.PlatformId,
                        PlatformName = request.PlatformName,
                        PlatformLogo = platform.ResultObj.PlatformLogo,
                        PlatformUrl = request.PlatformUrl,
                        Status = request.Status,
                    };
                    TempData["WarningToast"] = true;
                    return View(platfromVm);
                }

                var status = await _platformApiService.UpdatePlatform(request);
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
                    TempData["WarningToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }

                TempData["SuccessToast"] = true;
                return RedirectToAction("Index", "Platform");
            }
            catch
            {
                TempData["WarningToast"] = true;
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int PlatformId)
        {
            try
            {
                var platform = await _platformApiService.GetPlatfromById(PlatformId);
                if (platform is ApiErrorResult<PlatfromVm> errorResult)
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
        public async Task<IActionResult> Delete(DeletePlatformRequest request)
        {
            try
            {


                var status = await _platformApiService.DeletePlatform(request);
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
                return RedirectToAction("Index", "Platform");

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
        public async Task<IActionResult> Create(CreatePlatformRequest request)
        {


            var status = await _platformApiService.CreatePlatform(request);

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
                TempData["WarningToast"] = true;
                ViewBag.Errors = listError;
                return View();
            }
            TempData["SuccessToast"] = true;
            return RedirectToAction("Index", "Platform");
        }


    }

}
