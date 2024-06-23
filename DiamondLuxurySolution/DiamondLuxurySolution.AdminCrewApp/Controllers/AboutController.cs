using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Service.About;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;


namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
   
    [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

    public class AboutController : BaseController
    {
        private readonly IAboutApiService _AboutApiService;
        public AboutController(IAboutApiService aboutApiService)
        {
            _AboutApiService = aboutApiService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(ViewAboutRequest request)
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

                var about = await _AboutApiService.ViewAboutInManager(request);
                return View(about.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int aboutID)
        {
            try
            {
                var status = await _AboutApiService.GetAboutById(aboutID);
                if (status is ApiErrorResult<AboutVm> errorResult)
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
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int aboutId)
        {
            try
            {
                var platform = await _AboutApiService.GetAboutById(aboutId);
                if (platform is ApiErrorResult<AboutVm> errorResult)
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
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(platform.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAboutRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {

                    AboutVm aboutVm = new AboutVm()
                    {
                        AboutId = request.AboutId,
                        AboutName = request.AboutName,
                        Description = request.Description,
                        Status = request.Status,
                        AboutPhoneNumber = request.AboutPhoneNumber,
                        AboutAddress = request.AboutAddress,    
                        AboutEmail = request.AboutEmail 
                    };
                    return View(aboutVm);
                }
                var status = await _AboutApiService.UpdateAbout(request);
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
                return RedirectToAction("Index", "About");
            }
            catch
            {
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int aboutId)
        {
            try
            {
                var platform = await _AboutApiService.GetAboutById(aboutId);
                if (platform is ApiErrorResult<AboutVm> errorResult)
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
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(platform.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteAboutRequest request)
        {
            try
            {


                var status = await _AboutApiService.DeleteAbout(request);
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
                return RedirectToAction("Index", "About");

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
        public async Task<IActionResult> Create(CreateAboutRequest request)
        {

            var status = await _AboutApiService.CreateAbout(request);
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
            TempData["SuccessMsg"] = "Create success for Role " + request.AboutName;
            return RedirectToAction("Index", "About");
        }
    }
}
