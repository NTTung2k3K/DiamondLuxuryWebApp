using DiamondLuxurySolution.AdminCrewApp.Service.Contact;
using DiamondLuxurySolution.AdminCrewApp.Service.Gem;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class GemController : Controller
    {
        private readonly IGemApiService _gemApiService;

        public GemController(IGemApiService gemApiService)
        {
            _gemApiService = gemApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewGemRequest request)
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

                var gem = await _gemApiService.ViewGemInManager(request);
                return View(gem.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid GemId)
        {
            try
            {
                var gem = await _gemApiService.GetGemById(GemId);
                if (gem is ApiErrorResult<GemVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (gem.Message != null)
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
                return View(gem.ResultObj);
            }
            catch

            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateGemResquest request)
        {
            try
            {

                var status = await _gemApiService.UpdateGem(request);
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
                return RedirectToAction("Index", "Gem");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid GemId)
        {
            try
            {
                var status = await _gemApiService.GetGemById(GemId);
                if (status is ApiErrorResult<GemVm> errorResult)
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
        public async Task<IActionResult> Delete(Guid GemId)
        {
            try
            {
                var gem = await _gemApiService.GetGemById(GemId);
                if (gem is ApiErrorResult<GemVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (gem.Message != null)
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
                return View(gem.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteGemRequest request)
        {
            try
            {


                var status = await _gemApiService.DeleteGem(request);
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
                return RedirectToAction("Index", "Gem");

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
        public async Task<IActionResult> Create(CreateGemRequest request)
        {


            var status = await _gemApiService.CreateGem(request);

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
            TempData["SuccessMsg"] = "Create success for Role " + request.GemName;

            return RedirectToAction("Index", "Gem");
        }
    }
}
