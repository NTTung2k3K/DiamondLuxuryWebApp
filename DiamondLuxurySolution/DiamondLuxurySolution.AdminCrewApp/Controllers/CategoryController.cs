using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Service.Category;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Category;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;
namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{

	public class CategoryController : BaseController
	{
		private readonly ICategoryApiService _categoryApiService;

		public CategoryController(ICategoryApiService categoryApiService)
		{
			_categoryApiService = categoryApiService;
		}
		[Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]


		[HttpGet]
		public async Task<IActionResult> Index(ViewCategoryRequest request)
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

				var category = await _categoryApiService.ViewCategoryInManager(request);
				return View(category.ResultObj);
			}
			catch
			{
				return View();
			}
		}
		[Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

		[HttpGet]
		public async Task<IActionResult> Detail(int CategoryId)
		{
			try
			{
				var status = await _categoryApiService.GetCategoryById(CategoryId);
				if (status is ApiErrorResult<CategoryVm> errorResult)
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
		[Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]
        [HttpGet]
		public async Task<IActionResult> Edit(int CategoryId)
		{
			try
			{
				var category = await _categoryApiService.GetCategoryById(CategoryId);
				if (category is ApiErrorResult<CategoryVm> errorResult)
				{
					List<string> listError = new List<string>();
					if (category.Message != null)
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
				return View(category.ResultObj);
			}
			catch
			{
                TempData["ErrorToast"] = true;
                return View();
			}
		}
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
		public async Task<IActionResult> Edit(UpdateCategoryRequest request)
		{
			try
			{
				var category = await _categoryApiService.GetCategoryById(request.CategoryId);

                if (!ModelState.IsValid)
                {

                    CategoryVm categoryVm = new CategoryVm()
                    {
                        CategoryId = request.CategoryId,
						CategoryName = request.CategoryName,
						CategoryType = request.CategoryType,
						CategoryImage = category.ResultObj.CategoryImage,
                        Status = request.Status,
                        
                    };
                    TempData["WarningToast"] = true;
                    return View(categoryVm);
                }


                var status = await _categoryApiService.UpdateCategory(request);
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
                return RedirectToAction("Index", "Category");
			}
			catch
			{
                TempData["WarningToast"] = true;
                return View();
			}
		}


        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
		public async Task<IActionResult> Delete(int CategoryId)
		{
			try
			{
				var category = await _categoryApiService.GetCategoryById(CategoryId);
				if (category is ApiErrorResult<CategoryVm> errorResult)
				{
					List<string> listError = new List<string>();
					if (category.Message != null)
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
				return View(category.ResultObj);
			}
			catch
			{
                TempData["WarningToast"] = true;
                return View();
			}
		}
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
		public async Task<IActionResult> Delete(DeleteCategoryRequest request)
		{
			try
			{


				var status = await _categoryApiService.DeleteCategory(request);
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
                return RedirectToAction("Index", "Category");

			}
			catch
			{
                TempData["WarningToast"] = true;
                return View();
			}
		}
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
		public async Task<IActionResult> Create()
		{

			return View();
		}
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
		public async Task<IActionResult> Create(CreateCategoryRequest request)
		{


			var status = await _categoryApiService.CreateCategory(request);

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
            return RedirectToAction("Index", "Category");
		}


	}

}
