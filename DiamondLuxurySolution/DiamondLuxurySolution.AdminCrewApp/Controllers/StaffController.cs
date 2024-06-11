using DiamondLuxurySolution.AdminCrewApp.Service.Role;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class StaffController : BaseController
    {

        private readonly IStaffApiService _staffApiService;
        private readonly IRoleApiService _roleApiService;

        public StaffController(IStaffApiService staffApiService, IRoleApiService roleApiService)
        {
            _staffApiService = staffApiService;
            _roleApiService = roleApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var statuses = Enum.GetValues(typeof(StaffStatus)).Cast<StaffStatus>().ToList();
            ViewBag.ListStatus = statuses;

            var listRoleAll = await _roleApiService.GetRolesForView();
            ViewBag.ListRoleAll = listRoleAll.ResultObj.ToList();
            

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateStaffAccountRequest request)
        {
            var statuses = Enum.GetValues(typeof(StaffStatus)).Cast<StaffStatus>().ToList();
            ViewBag.ListStatus = statuses;

            var listRoleAll = await _roleApiService.GetRolesForView();
            ViewBag.ListRoleAll = listRoleAll.ResultObj.ToList();
            

            if (!ModelState.IsValid)
            {
                
                return View(request);
            }

            var status = await _staffApiService.RegisterStaffAccount(request);

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

            TempData["SuccessMsg"] = "Tạo mới thành công cho " + request.FullName;

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Login");
        }
    }
}
