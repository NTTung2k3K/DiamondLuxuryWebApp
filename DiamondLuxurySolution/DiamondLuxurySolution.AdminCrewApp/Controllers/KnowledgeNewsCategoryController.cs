using DiamondLuxurySolution.AdminCrewApp.Service.About;
using DiamondLuxurySolution.AdminCrewApp.Service.KnowledgeNewsCategoty;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

    public class KnowledgeNewsCategoryController : BaseController
    {
        private readonly IKnowledgeNewsCategoryApiService _knowledgeNewsCategoryApiService;
        public KnowledgeNewsCategoryController(IKnowledgeNewsCategoryApiService knowledgeNewsCategoryApiService)
        {
            _knowledgeNewsCategoryApiService = knowledgeNewsCategoryApiService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(ViewKnowledgeNewsCategoryRequest request)
        {
            try
            {

                ViewBag.txtLastSeachValue = request.KeyWord;
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

                var platform = await _knowledgeNewsCategoryApiService.ViewKnowledgeNewsCategory(request);
                return View(platform.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int knowledgeNewsCategoryId)
        {
            try
            {
                var status = await _knowledgeNewsCategoryApiService.GetKnowledgeNewsCategoryById(knowledgeNewsCategoryId);
                if (status is ApiErrorResult<KnowledgeNewsCategoryVm> errorResult)
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
        public async Task<IActionResult> Edit(int knowledgeNewsCategoryId)
        {
            try
            {
                var platform = await _knowledgeNewsCategoryApiService.GetKnowledgeNewsCategoryById(knowledgeNewsCategoryId);
                if (platform is ApiErrorResult<KnowledgeNewsCategoryVm> errorResult)
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
        public async Task<IActionResult> Edit(UpdateKnowledgeNewsCategoryRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {

                    KnowledgeNewsCategoryVm knowledgeNewsCategoryVm = new KnowledgeNewsCategoryVm()
                    {
                       KnowledgeNewCatagoryId=request.KnowledgeNewCatagoryId,
                       KnowledgeNewCatagoriesName=request.KnowledgeNewCatagoriesName,
                       Description=request.Description,

                    };
                    return View(knowledgeNewsCategoryVm);
                }
                var status = await _knowledgeNewsCategoryApiService.UpdateKnowledgeNewsCategory(request);
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
                return RedirectToAction("Index", "KnowledgeNewsCategory");
            }
            catch
            {
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int knowledgeNewsCategoryId)
        {
            try
            {
                var platform = await _knowledgeNewsCategoryApiService.GetKnowledgeNewsCategoryById(knowledgeNewsCategoryId);
                if (platform is ApiErrorResult<KnowledgeNewsCategoryVm> errorResult)
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
        public async Task<IActionResult> Delete(DeleteKnowledgeNewsCategoryRequest request)
        {
            try
            {


                var status = await _knowledgeNewsCategoryApiService.DeleteKnowledgeNewsCategory(request);
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
                return RedirectToAction("Index", "KnowledgeNewsCategory");

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
        public async Task<IActionResult> Create(CreateKnowledgeNewsCategoryRequest request)
        {
            var status = await _knowledgeNewsCategoryApiService.CreateKnowledgeNewsCategory(request);

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
            TempData["SuccessMsg"] = "Create success for Role " + request.KnowledgeNewCatagoriesName;

            return RedirectToAction("Index", "KnowledgeNewsCategory");
        }
    }
}
