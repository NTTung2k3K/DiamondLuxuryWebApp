using DiamondLuxurySolution.AdminCrewApp.Service.KnowledgeNews;
using DiamondLuxurySolution.AdminCrewApp.Service.KnowledgeNewsCategoty;
using DiamondLuxurySolution.AdminCrewApp.Service.News;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.ViewModel.Models.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

    public class KnowledgeNewsController : BaseController
    {
        private readonly IKnowLedgeNewsApiService _knowledgeNewsApiService;
        private readonly IKnowledgeNewsCategoryApiService _knowledgeNewsCategoryApiService;
        private readonly IStaffApiService _staffApiService;

        public KnowledgeNewsController(IKnowLedgeNewsApiService knowledgeNewsApiService, IStaffApiService staffApiService, IKnowledgeNewsCategoryApiService knowledgeNewsCategory)
        {
            _staffApiService = staffApiService;
            _knowledgeNewsApiService = knowledgeNewsApiService;
            _knowledgeNewsCategoryApiService = knowledgeNewsCategory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewKnowledgeNewsRequest request)
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

                var knowledgeNews = await _knowledgeNewsApiService.ViewKnowledgeNews(request);
                return View(knowledgeNews.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int KnowLedgeNewsId)
        {
            try
            {
                var status = await _knowledgeNewsApiService.GetKnowledgeNewsById(KnowLedgeNewsId);
                if (status is ApiErrorResult<KnowledgeNewsVm> errorResult)
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
        public async Task<IActionResult> Delete(int KnowLedgeNewsId)
        {
            try
            {
                var News = await _knowledgeNewsApiService.GetKnowledgeNewsById(KnowLedgeNewsId);
                if (News is ApiErrorResult<KnowledgeNewsVm> errorResult)
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
        public async Task<IActionResult> Delete(DeleteKnowledgeNewsRequest request)
        {
            try
            {
                var status = await _knowledgeNewsApiService.DeleteKnowledgeNews(request);
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

                return RedirectToAction("Index", "KnowledgeNews");

            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int KnowLedgeNewsId)
        {
            try
            {
                var listKnowledgeNewsCategory = await _knowledgeNewsCategoryApiService.GetAll();
                ViewBag.ListKnow = listKnowledgeNewsCategory.ResultObj.ToList();

                var News = await _knowledgeNewsApiService.GetKnowledgeNewsById(KnowLedgeNewsId);
                if (News is ApiErrorResult<KnowledgeNewsVm> errorResult)
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
        public async Task<IActionResult> Edit(UpdateKnowledgeNewsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var saveIMG = await _knowledgeNewsApiService.GetKnowledgeNewsById(request.KnowledgeNewsId);

                    var listKnowledgeNewsCategory = await _knowledgeNewsCategoryApiService.GetAll();
                    ViewBag.ListKnow = listKnowledgeNewsCategory.ResultObj.ToList();

                    var writer = await _staffApiService.GetStaffById((Guid)request.WriterId);
                    var knoledgeNewsCategory = await _knowledgeNewsCategoryApiService.GetKnowledgeNewsCategoryById((int)request.KnowledgeNewCatagoryId);
                    var knowledgeNewsVmCall = await _knowledgeNewsApiService.GetKnowledgeNewsById(request.KnowledgeNewsId);

                    KnowledgeNewsVm knowledgeNewsVm = new KnowledgeNewsVm()
                    {
                        KnowledgeNewsId = request.KnowledgeNewsId,
                        Active = request.Active,
                        Description = request.Description,
                        KnowledgeNewsName = request.KnowledgeNewsName,
                        KnowledgeNewCatagoryVm = knoledgeNewsCategory.ResultObj,
                        Writer = writer.ResultObj,
                        DateCreated = saveIMG.ResultObj.DateCreated,
                        DateModified=saveIMG.ResultObj.DateModified,
                        Thumnail= saveIMG.ResultObj.Thumnail,
                    };
                    return View(knowledgeNewsVm);
                }
                var writerId = _knowledgeNewsApiService.GetKnowledgeNewsById(request.KnowledgeNewsId);
                request.WriterId = writerId.Result.ResultObj.Writer.StaffId;
                var status = await _knowledgeNewsApiService.UpdateKnowledgeNews(request);
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
                    var listKnowledgeNewsCategory = await _knowledgeNewsCategoryApiService.GetAll();
                    ViewBag.ListKnow = listKnowledgeNewsCategory.ResultObj.ToList();
                    return View(request);
                }

                return RedirectToAction("Index", "KnowledgeNews");
            }
            catch
            {
                return View(request);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var listKnowledgeNewsCategory = await _knowledgeNewsCategoryApiService.GetAll();
            ViewBag.ListKnow = listKnowledgeNewsCategory.ResultObj.ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateKnowledgeNewsRequest request)
        {
            var listKnowledgeNewsCategory = await _knowledgeNewsCategoryApiService.GetAll();
            ViewBag.ListKnow = listKnowledgeNewsCategory.ResultObj.ToList();


            

            string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.USER_ID);
            Guid userId;

            Guid.TryParse(userIdString, out userId);
            // Chuyển đổi thành công
            request.WriterId = userId;
            request.DateCreated = DateTime.Now;
            request.DateModified = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var status = await _knowledgeNewsApiService.CreateKnowledgeNews(request);

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

            TempData["SuccessMsg"] = "Tạo mới thành công cho " + request.KnowledgeNewsName;

            return RedirectToAction("Index", "KnowledgeNews");
        }


    }
}
