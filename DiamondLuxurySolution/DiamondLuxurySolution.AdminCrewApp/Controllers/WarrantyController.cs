using DiamondLuxurySolution.AdminCrewApp.Service.Material;
using DiamondLuxurySolution.AdminCrewApp.Service.Warranty;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using DiamondLuxurySolution.ViewModel.Models.Warranty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
	public class WarrantyController : BaseController
    {
		private readonly IWarrantyApiService _warrantyApiService;

		public WarrantyController(IWarrantyApiService warrantyApiService)
		{
			_warrantyApiService = warrantyApiService;
		}

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
		public async Task<IActionResult> Index(ViewWarrantyRequest request)
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

				var material = await _warrantyApiService.ViewWarrantyInManager(request);
				return View(material.ResultObj);
			}
			catch
			{
				return View();
			}
		}
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Detail(string WarrantyId)
        {
            try
            {
                var status = await _warrantyApiService.GetWarrantyById(WarrantyId);
                if (status is ApiErrorResult<WarrantyVm> errorResult)
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
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpGet]
        public async Task<IActionResult> Edit(string WarrantyId)
        {
            try
            {
                var Warranty = await _warrantyApiService.GetWarrantyById(WarrantyId);
                if (Warranty is ApiErrorResult<WarrantyVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Warranty.Message != null)
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
                return View(Warranty.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateWarrantyRequest request)
        {
            try
            {
                var warranty = await _warrantyApiService.GetWarrantyById(request.WarrantyId);

                if (!ModelState.IsValid)
                {
                    
                    WarrantyVm warrantyVm = new WarrantyVm()
                    {
                        WarrantyId = request.WarrantyId,
                        Description = request.Description,
                        Status = request.Status,
                        WarrantyName = request.WarrantyName,
                        DateActive = warranty.ResultObj.DateActive,
                        DateExpired = warranty.ResultObj.DateExpired,
                    };
                    TempData["WarningToast"] = true;
                    return View(warrantyVm);
                }

                var status = await _warrantyApiService.UpdateWarranty(request);
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
                    WarrantyVm warrantyVm = new WarrantyVm()
                    {
                        WarrantyId = request.WarrantyId,
                        Description = request.Description,
                        Status = request.Status,
                        WarrantyName = request.WarrantyName,
                        DateActive = warranty.ResultObj.DateActive,
                        DateExpired = warranty.ResultObj.DateExpired,
                    };
                    TempData["WarningToast"] = true;
                    return View(warrantyVm);

                }
                return RedirectToAction("Index", "Warranty");
            }
            catch
            {
                TempData["WarningToast"] = true;
                return View();
            }
        }

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpGet]
        public async Task<IActionResult> Delete(string WarrantyId)
        {
            try
            {
                var Warranty = await _warrantyApiService.GetWarrantyById(WarrantyId);
                if (Warranty is ApiErrorResult<WarrantyVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Warranty.Message != null)
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
                return View(Warranty.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteWarrantyRequest request)
        {
            try
            {
                var status = await _warrantyApiService.DeleteWarranty(request);
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
                return RedirectToAction("Index", "Warranty");
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }


        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpPost]
        public async Task<IActionResult> Create(CreateWarrantyRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["WarningToast"] = true;
                return View(request);
            }

            var status = await _warrantyApiService.CreateWarranty(request);

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

            return RedirectToAction("Index", "Warranty");
        }
    }
}
