using DiamondLuxurySolution.AdminCrewApp.Service.Role;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{

    [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

    public class RoleController : BaseController
    {
        private readonly IRoleApiService _roleApiService;

        public RoleController(IRoleApiService roleApiService)
        {
            _roleApiService = roleApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewRoleRequest request)
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

                var Role = await _roleApiService.GetRolePagination(request);
                return View(Role.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid RoleId)
        {
            try
            {
                var status = await _roleApiService.GetRoleById(RoleId);
                if (status is ApiErrorResult<RoleVm> errorResult)
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid RoleId)
        {
            try
            {
                var Role = await _roleApiService.GetRoleById(RoleId);
                if (Role is ApiErrorResult<RoleVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Role.Message != null)
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
                return View(Role.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    RoleVm roleVm = new RoleVm()
                    {
                        Description = request.Description,
                        Name = request.Name,
                        RoleId = request.RoleId,
                    };
                    TempData["WarningToast"] = true;
                    return View(roleVm);
                }

                var status = await _roleApiService.UpdateRole(request);
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
                return RedirectToAction("Index", "Role");
            }
            catch
            {
                TempData["WarningToast"] = true;
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(Guid RoleId)
        {
            try
            {
                var Role = await _roleApiService.GetRoleById(RoleId);
                if (Role is ApiErrorResult<RoleVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Role.Message != null)
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
                return View(Role.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteRoleRequest request)
        {
            try
            {
                var status = await _roleApiService.DeleteRole(request);
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
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }
                TempData["SuccessToast"] = true;
                return RedirectToAction("Index", "Role");

            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["WarningToast"] = true;
                return View(request);
            }

            var status = await _roleApiService.CreateRole(request);

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

            return RedirectToAction("Index", "Role");
        }
    }
}
