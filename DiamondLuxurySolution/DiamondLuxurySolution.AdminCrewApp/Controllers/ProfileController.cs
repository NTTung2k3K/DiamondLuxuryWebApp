using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Service.Role;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin + ", "
                + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.DeliveryStaff
                + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]
    public class ProfileController : BaseController
    {
        private readonly IStaffApiService _staffApiService;
        private readonly IRoleApiService _roleApiService;

        public ProfileController(IStaffApiService staffApiService, IRoleApiService roleApiService)
        {
            _staffApiService = staffApiService;
            _roleApiService = roleApiService;
        }
        public async Task<IActionResult> Detail()
        {
            string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.USER_ID);
            Guid userId;

            Guid.TryParse(userIdString, out userId);
            // Chuyển đổi thành công
            var staff = await _staffApiService.GetStaffById(userId);
            if (staff is ApiErrorResult<StaffVm> errorResult)
            {
                TempData["ErrorToast"] = true;
                return View(errorResult);
            }
            return View(staff.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(Guid StaffId)
        {
            try
            {
                string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.USER_ID);
                Guid userId;

                Guid.TryParse(userIdString, out userId);
                // Chuyển đổi thành công
                var staff = await _staffApiService.GetStaffById(userId);
                if (staff is ApiErrorResult<StaffVm> errorResult)
                {
                    TempData["WarningToast"] = true;
                    return View(errorResult);
                }
                return View(staff.ResultObj);
            }
            catch
            {
                TempData["WarningToast"] = true;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordStaffRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var staff = await _staffApiService.GetStaffById(request.StaffId);
                    return View(staff);
                }


                var status = await _staffApiService.ChangePasswordStaff(request);
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
                    var listRoleName = new List<string>();
                    var staff = await _staffApiService.GetStaffById(request.StaffId);
                    return View(staff.ResultObj);
                }
                TempData["SuccessToast"] = true;
                return RedirectToAction("Detail", "Profile");
            }
            catch
            {
                TempData["WarningToast"] = true;
                return View(request);
            }
        }

    }
}
