using DiamondLuxurySolution.AdminCrewApp.Service.Customer;
using DiamondLuxurySolution.AdminCrewApp.Service.Role;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin)]
    public class CustomerController : BaseController
    {
        private readonly IStaffApiService _staffApiService;
        private readonly IRoleApiService _roleApiService;
        private readonly ICustomerApiService _customerApiService;


        public CustomerController(IStaffApiService staffApiService, IRoleApiService roleApiService, ICustomerApiService customerApiService)
        {
            _staffApiService = staffApiService;
            _roleApiService = roleApiService;
            _customerApiService = customerApiService;
        }

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

                var Staff = await _staffApiService.ViewCustomerPagination(request);
                return View(Staff.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid CustomerId)
        {
            try
            {
                var status = await _customerApiService.GetCustomerById(CustomerId);
                if (status is ApiErrorResult<CustomerVm> errorResult)
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
        public async Task<IActionResult> Edit(Guid CustomerId)
        {
            try
            {
                var statuses = Enum.GetValues(typeof(CustomerStatus)).Cast<CustomerStatus>().ToList();
                ViewBag.ListStatus = statuses;

                var listRoleAll = await _roleApiService.GetRolesForView();
                ViewBag.ListRoleAll = listRoleAll.ResultObj.ToList();

                var Staff = await _customerApiService.GetCustomerById(CustomerId);
                if (Staff is ApiErrorResult<CustomerVm> errorResult)
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

                CustomerVm staffVm = new CustomerVm()
                {
                    Address = Staff.ResultObj.Address,
                    Dob = (DateTime)Staff.ResultObj.Dob,
                    Email = Staff.ResultObj.Email,
                    FullName = Staff.ResultObj.FullName,
                    PhoneNumber = Staff.ResultObj.PhoneNumber,
                    ListRoleName = Staff.ResultObj.ListRoleName,
                    Status = Staff.ResultObj.Status,
                };
                return View(staffVm);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateStaffAccountRequest request)
        {
            try
            {
                var statuses = Enum.GetValues(typeof(CustomerStatus)).Cast<CustomerStatus>().ToList();
                ViewBag.ListStatus = statuses;

                var listRoleAll = await _roleApiService.GetRolesForView();
                ViewBag.ListRoleAll = listRoleAll.ResultObj.ToList();
                var changeRequest = new ChangeStatusCustomerRequest()
                {
                    Status = request.Status,
                    Email = request.Email,
                };
                var status = await _staffApiService.ChangeStatusCustomer(changeRequest);
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
                    CustomerVm staffVm = new CustomerVm()
                    {
                        Address = request.Address,
                        Dob = request.Dob,
                        Email = request.Email,
                        FullName = request.FullName,
                        PhoneNumber = request.PhoneNumber,
                        ListRoleName = request.ListRoleName,
                        Status = request.Status,
                    };
                    if (listRoleName.Count > 0)
                    {
                        staffVm.ListRoleName = listRoleName;
                    }
                    return View(staffVm);

                }

                return RedirectToAction("Index", "Customer");
            }
            catch
            {
                return View(request);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(Guid CustomerId)
        {
            try
            {
                var Staff = await _customerApiService.GetCustomerById(CustomerId);
                if (Staff is ApiErrorResult<CustomerVm> errorResult)
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

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCustomerRequest request)
        {
            try
            {
                var status = await _staffApiService.DeleteStaff(request.CustomerId);
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

                return RedirectToAction("Index", "Customer");

            }
            catch
            {
                return View();
            }
        }
    }
}
