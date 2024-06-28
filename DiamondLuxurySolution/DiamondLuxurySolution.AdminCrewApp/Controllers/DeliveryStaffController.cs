using DiamondLuxurySolution.AdminCrewApp.Service.Order;
using DiamondLuxurySolution.AdminCrewApp.Service.Role;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{

    public class DeliveryStaffController : BaseController
    {
        private readonly IStaffApiService _staffApiService;
        private readonly IRoleApiService _roleApiService;
        private readonly IOrderApiService _OrderApiService;

        public DeliveryStaffController(IStaffApiService staffApiService, IRoleApiService roleApiService, IOrderApiService OrderApiService)
        {
            _staffApiService = staffApiService;
            _roleApiService = roleApiService;
            _OrderApiService = OrderApiService;
        }

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.DeliveryStaff)]
        [HttpGet]
        public async Task<IActionResult> IndexOrder(ViewOrderForDeliveryStaff request)
        {
            string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.USER_ID);
            Guid userId;

            Guid.TryParse(userIdString, out userId);
            // Chuyển đổi thành công
            request.shipperId = userId;

            var shipper = await _staffApiService.GetStaffById(userId);
            ViewBag.ShipperWorking = shipper.ResultObj.ShipStatus;
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

                var Order = await _staffApiService.ViewOrderForDeliveryStaff(request);
                return View(Order.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditOrder(UpdateStatusOrderForDeliveryStaff request)
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    return View(request);
                }
                
                var status = await _staffApiService.UpdateStatusOrderForDeliveryStaff(request);
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
                    return View(request);
                }
                TempData["SuccessToast"] = true;
                return RedirectToAction("IndexOrder", "DeliveryStaff");
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View(request);
            }
        }

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.DeliveryStaff)]

        [HttpGet]
        public async Task<IActionResult> DetailOrder(string OrderId)
        {
            try
            {
                var Order = await _OrderApiService.GetOrderById(OrderId);
                ViewBag.PaymentList = Order.ResultObj.OrdersPaymentVm;

                if (Order is ApiErrorResult<OrderVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Order.Message != null)
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
                return View(Order.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin)]


        [HttpGet]
        public async Task<IActionResult> Edit(Guid StaffId)
        {
            try
            {
                var statuses = Enum.GetValues(typeof(StaffStatus)).Cast<StaffStatus>().ToList();
                ViewBag.ListStatus = statuses;

                var listRoleAll = await _roleApiService.GetRolesForView();
                ViewBag.ListRoleAll = listRoleAll.ResultObj.ToList();

                var Staff = await _staffApiService.GetStaffById(StaffId);
                if (Staff is ApiErrorResult<StaffVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Staff.Message != null)
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
                var listRoleName = new List<string>();

                StaffVm staffVm = new StaffVm()
                {
                    Address = Staff.ResultObj.Address,
                    CitizenIDCard = Staff.ResultObj.CitizenIDCard,
                    StaffId = Staff.ResultObj.StaffId,
                    Dob = (DateTime)Staff.ResultObj.Dob,
                    Email = Staff.ResultObj.Email,
                    FullName = Staff.ResultObj.FullName,
                    Image = Staff.ResultObj.Image,
                    PhoneNumber = Staff.ResultObj.PhoneNumber,
                    ListRoleName = Staff.ResultObj.ListRoleName,
                    Status = Staff.ResultObj.Status,
                    Username = Staff.ResultObj.Username
                };
                return View(staffVm);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin)]

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateStaffAccountRequest request)
        {
            try
            {
                var statuses = Enum.GetValues(typeof(StaffStatus)).Cast<StaffStatus>().ToList();
                ViewBag.ListStatus = statuses;

                var listRoleAll = await _roleApiService.GetRolesForView();
                ViewBag.ListRoleAll = listRoleAll.ResultObj.ToList();


                if (!ModelState.IsValid)
                {
                    var listRoleName = new List<string>();
                    foreach (var item in request.RoleId)
                    {
                        var role = _roleApiService.GetRoleById(item);
                        listRoleName.Add(role.Result.ResultObj.Name);
                    }
                    StaffVm staffVm = new StaffVm()
                    {
                        Username = request.Username,
                        Address = request.Address,
                        CitizenIDCard = request.CitizenIDCard,
                        Dob = request.Dob,
                        Email = request.Email,
                        FullName = request.FullName,
                        ListRoleName = request.ListRoleName,
                        PhoneNumber = request.PhoneNumber,
                        RoleId = request.RoleId,
                        StaffId = request.StaffId,
                        Status = request.Status
                    };
                    if (listRoleName.Count > 0)
                    {
                        staffVm.ListRoleName = listRoleName;
                    }
                    return View(staffVm);
                }


                var status = await _staffApiService.UpdateStaffAccount(request);
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
                    var listRoleName = new List<string>();
                    foreach (var item in request.RoleId)
                    {
                        var role = _roleApiService.GetRoleById(item);
                        listRoleName.Add(role.Result.ResultObj.Name);
                    }
                    StaffVm staffVm = new StaffVm()
                    {
                        Username = request.Username,
                        Address = request.Address,
                        CitizenIDCard = request.CitizenIDCard,
                        Dob = request.Dob,
                        Email = request.Email,
                        FullName = request.FullName,
                        ListRoleName = request.ListRoleName,
                        PhoneNumber = request.PhoneNumber,
                        RoleId = request.RoleId,
                        StaffId = request.StaffId,
                        Status = request.Status
                    };
                    if (listRoleName.Count > 0)
                    {
                        staffVm.ListRoleName = listRoleName;
                    }
                    return View(staffVm);

                }

                return RedirectToAction("Index", "DeliveryStaff");
            }
            catch
            {
                return View(request);
            }
        }

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin)]


        [HttpGet]
        public async Task<IActionResult> Delete(Guid StaffId)
        {
            try
            {
                var Staff = await _staffApiService.GetStaffById(StaffId);
                if (Staff is ApiErrorResult<StaffVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Staff.Message != null)
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
                return View(Staff.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin)]

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteStaffAccountRequest request)
        {
            try
            {
                var status = await _staffApiService.DeleteStaff(request.StaffId);
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

                return RedirectToAction("Index", "DeliveryStaff");

            }
            catch
            {
                return View();
            }
        }


        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin)]
        [HttpGet]
        public async Task<IActionResult> Index(ViewStaffPaginationCommonRequest request)
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

                var Staff = await _staffApiService.ViewDeliveryStaffPagination(request);
                return View(Staff.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin)]

        [HttpGet]
        public async Task<IActionResult> Detail(Guid StaffId)
        {
            try
            {
                var status = await _staffApiService.GetStaffById(StaffId);
                if (status is ApiErrorResult<StaffVm> errorResult)
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

       

    }
}
