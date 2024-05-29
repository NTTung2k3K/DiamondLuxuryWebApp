using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Service.Platform;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class PlatformController : Controller
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

                if (!ModelState.IsValid)
                {
                    return View();
                }
                var status = await _platformApiService.GetPlatfromById(PlatformId);
                if (status is ApiErrorResult<PlatfromVm> errorResult)
                {
                    ViewBag.Error = "Không thể tìm thấy " + PlatformId.ToString();
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
        public async Task<IActionResult> Edit(int PlatformId)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }

                var platform = await _platformApiService.GetPlatfromById(PlatformId);
                return View(platform.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdatePlatformRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }
                var status = await _platformApiService.UpdatePlatform(request);
                  
                return RedirectToAction("Index", "Platform");
            }
            catch
            {
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int PlatformId)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }

                var User = await _platformApiService.GetPlatfromById(PlatformId);
                return View(User.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeletePlatformRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }
                var status = await _platformApiService.DeletePlatform(request);
                if (status is ApiErrorResult<bool> errorResult)
                {
                    ViewBag.Errors = errorResult.ValidationErrors;
                    return View();
                }
                return RedirectToAction("Index", "Platform");

            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var platformVm = new DiamondLuxurySolution.ViewModel.Models.Platform.CreatePlatformRequest()
            {
                PlatformName = "",
                PlatformUrl = "",
            };
            return View(platformVm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePlatformRequest request)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            var status = await _platformApiService.CreatePlatform(request);

            if (status is ApiErrorResult<bool> errorResult)
            {
                ViewBag.Errors = errorResult.Message;
                return View();
            }
            TempData["SuccessMsg"] = "Create success for Role " + request.PlatformName;

            return RedirectToAction("Index", "Platform");
        }


    }

}
