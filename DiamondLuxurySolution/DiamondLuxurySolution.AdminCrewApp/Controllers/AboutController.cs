using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Service.About;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;


namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutApiService _AboutApiService;
        public AboutController(IAboutApiService aboutApiService)
        {
            _AboutApiService = aboutApiService;
        }




        [HttpGet]
        public async Task<IActionResult> TestView(ViewAboutRequest request)
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

                // Chuyển hướng người dùng đến action Create
            }
            catch
            {
                return View();
            }
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
                return View();
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

        [HttpPost]
        public async Task<IActionResult> SaveSelectedId(string selectedIds)
        {
            try
            {
                HttpContext.Session.SetString("SelectedIds", selectedIds);
                return RedirectToAction("Create", "About");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var selectedIdsString = HttpContext.Session.GetString("SelectedIds");
            List<int> listSelectedId = new List<int>();
            if (selectedIdsString.Any())
            {
                // Tách chuỗi thành các phần tử riêng lẻ và chuyển đổi thành số nguyên
                var selectedIdsArray = selectedIdsString.Split(',').Select(int.Parse).ToList();
                // Giờ bạn có thể sử dụng selectedIdsArray như là một danh sách List<int>
                listSelectedId = selectedIdsArray.ToList();
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAboutRequest request)
        {
            var selectedIdsString = HttpContext.Session.GetString("SelectedIds");
            List<int> listSelectedId = new List<int>();
            if (selectedIdsString.Any())
            {
                // Tách chuỗi thành các phần tử riêng lẻ và chuyển đổi thành số nguyên
                var selectedIdsArray = selectedIdsString.Split(',').Select(int.Parse).ToList();
                // Giờ bạn có thể sử dụng selectedIdsArray như là một danh sách List<int>
                listSelectedId = selectedIdsArray.ToList();
            }
            // Sau khi có danh sách số nguyên, bạn có thể gán cho request.ListId như sau:
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
