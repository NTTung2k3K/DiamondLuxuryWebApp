using DiamondLuxurySolution.AdminCrewApp.Service.News;
using DiamondLuxurySolution.AdminCrewApp.Service.Role;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.News;
using DiamondLuxurySolution.ViewModel.Models.Role;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsApiService _NewsApiService;
        private readonly IStaffApiService _staffApiService;

        public NewsController(INewsApiService NewsApiService,IStaffApiService staffApiService)
        {
            _staffApiService = staffApiService;
            _NewsApiService = NewsApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewNewsRequest request)
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

                var News = await _NewsApiService.ViewNews(request);
                return View(News.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int NewsId)
        {
            try
            {
                var status = await _NewsApiService.GetNewsById(NewsId);
                if (status is ApiErrorResult<NewsVm> errorResult)
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
        public async Task<IActionResult> Edit(int NewsId)
        {
            try
            {
                var News = await _NewsApiService.GetNewsById(NewsId);
                if (News is ApiErrorResult<NewsVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (News.Message != null)
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
                return View(News.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateNewsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var writer =  await _staffApiService.GetStaffById(request.WriterId);
                    var newsVmCall = await _NewsApiService.GetNewsById(request.NewsId);

                    NewsVm newsVm = new NewsVm()
                    {
                        NewsId = request.NewsId,
                        Description = request.Description,
                        NewName = request.NewName,
                        Status = request.Status,
                        Title = request.Title,
                        Writer = writer.ResultObj,
                        DateCreated = newsVmCall.ResultObj.DateCreated,
                        DateModified = newsVmCall.ResultObj.DateModified,
                        Image = newsVmCall.ResultObj.Image
                    };
                    return View(newsVm);
                }
                var writerId = _NewsApiService.GetNewsById(request.NewsId);
                request.WriterId = writerId.Result.ResultObj.Writer.StaffId;
                var status = await _NewsApiService.UpdateNews(request);
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
                    return View(request);
                }

                return RedirectToAction("Index", "News");
            }
            catch
            {
                return View(request);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int NewsId)
        {
            try
            {
                var News = await _NewsApiService.GetNewsById(NewsId);
                if (News is ApiErrorResult<NewsVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (News.Message != null)
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
                return View(News.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteNewsRequest request)
        {
            try
            {
                var status = await _NewsApiService.DeleteNews(request);
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

                return RedirectToAction("Index", "News");

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
        public async Task<IActionResult> Create(CreateNewsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.USER_ID);
            Guid userId;

            Guid.TryParse(userIdString, out userId);
            // Chuyển đổi thành công
             request.WriterId = userId;


            var status = await _NewsApiService.CreateNews(request);

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

            TempData["SuccessMsg"] = "Tạo mới thành công cho " + request.Title;

            return RedirectToAction("Index", "News");
        }
    }
}
