using DiamondLuxurySolution.AdminCrewApp.Service.WarrantyDetail;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.Application.Repository.WarrantyDetail;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.WarrantyDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]
    public class WarrantyDetailController : Controller
    {
        private readonly IWarrantyDetailService _WarrantyDetailApiService;
        private readonly IStaffApiService _staffApiService;

        public WarrantyDetailController(IWarrantyDetailService WarrantyDetailApiService, IStaffApiService staffApiService)
        {
            _staffApiService = staffApiService;
            _WarrantyDetailApiService = WarrantyDetailApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewWarrantyDetailRequest request)
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

                var WarrantyDetail = await _WarrantyDetailApiService.ViewWarrantyDetai(request);
                return View(WarrantyDetail.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int WarrantyDetailId)
        {
            try
            {
                var status = await _WarrantyDetailApiService.GetWarrantyDetaiById(WarrantyDetailId);
                if (status is ApiErrorResult<WarrantyDetailVm> errorResult)
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
        public async Task<IActionResult> Edit(int WarrantyDetailId)
        {
            try
            {
                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<WarrantyDetailStatus>().ToList();
                ViewBag.ListStatus = statuses;
                var WarrantyDetail = await _WarrantyDetailApiService.GetWarrantyDetaiById(WarrantyDetailId);

                if (WarrantyDetail is ApiErrorResult<WarrantyDetailVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (WarrantyDetail.Message != null)
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
                return View(WarrantyDetail.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateWarrantyDetailRequest request)
        {
            try
            {
                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<WarrantyDetailStatus>().ToList();
                ViewBag.ListStatus = statuses;

                var WarrantyDetailVmCall = await _WarrantyDetailApiService.GetWarrantyDetaiById(request.WarrantyDetailId);

                WarrantyDetailVm WarrantyDetailVm = new WarrantyDetailVm()
                {
                    WarrantyDetailId = WarrantyDetailVmCall.ResultObj.WarrantyDetailId,
                    Description = WarrantyDetailVmCall.ResultObj.Description,
                    ReceiveProductDate = WarrantyDetailVmCall.ResultObj.ReceiveProductDate,
                    ReturnProductDate = WarrantyDetailVmCall.ResultObj.ReturnProductDate ?? DateTime.MinValue,
                    Image = WarrantyDetailVmCall.ResultObj.Image,
                    WarrantyDetailName = WarrantyDetailVmCall.ResultObj.WarrantyDetailName,
                    WarrantyType = WarrantyDetailVmCall.ResultObj.WarrantyType,
                    ProductVm = new ProductVm()
                    {
                        ProductId = WarrantyDetailVmCall.ResultObj.ProductVm.ProductId,
                        ProductName = WarrantyDetailVmCall.ResultObj.ProductVm.ProductName,
                        Description = WarrantyDetailVmCall.ResultObj.ProductVm.Description,
                        ProductThumbnail = WarrantyDetailVmCall.ResultObj.ProductVm.ProductThumbnail,
                        IsHome = WarrantyDetailVmCall.ResultObj.ProductVm.IsHome,
                        IsSale = WarrantyDetailVmCall.ResultObj.ProductVm.IsSale,
                        PercentSale = WarrantyDetailVmCall.ResultObj.ProductVm.PercentSale,
                        Status = WarrantyDetailVmCall.ResultObj.ProductVm.Status,
                        Quantity = WarrantyDetailVmCall.ResultObj.ProductVm.Quantity,
                        OriginalPrice = WarrantyDetailVmCall.ResultObj.ProductVm.OriginalPrice,
                        SellingPrice = WarrantyDetailVmCall.ResultObj.ProductVm.SellingPrice,
                    },
                    CustomerVm = new CustomerVm()
                    {
                        CustomerId = WarrantyDetailVmCall.ResultObj.CustomerVm.CustomerId,
                        FullName = WarrantyDetailVmCall.ResultObj.CustomerVm.FullName ?? "Không có",
                        Email = WarrantyDetailVmCall.ResultObj.CustomerVm.Email ?? "Không có",
                        PhoneNumber = WarrantyDetailVmCall.ResultObj.CustomerVm.PhoneNumber ?? "Không có",
                        Address = WarrantyDetailVmCall.ResultObj.CustomerVm.Address ?? "Không có"
                    },
                    Status = WarrantyDetailVmCall.ResultObj.Status,
                    WarrantyVm = new ViewModel.Models.Warranty.WarrantyVm()
                    {
                        WarrantyId = WarrantyDetailVmCall.ResultObj.WarrantyVm.WarrantyId,
                        DateActive = WarrantyDetailVmCall.ResultObj.WarrantyVm.DateActive,
                        DateExpired = WarrantyDetailVmCall.ResultObj.WarrantyVm.DateExpired,
                        Description = WarrantyDetailVmCall.ResultObj.WarrantyVm.Description,
                        Status = WarrantyDetailVmCall.ResultObj.WarrantyVm.Status,
                        WarrantyName = WarrantyDetailVmCall.ResultObj.WarrantyVm.WarrantyName
                    }
                };
                var status = await _WarrantyDetailApiService.UpdateWarrantyDetail(request);
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
                    return View(WarrantyDetailVm);
                }

                return RedirectToAction("Index", "WarrantyDetail");
            }
            catch
            {
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int WarrantyDetailId)
        {
            try
            {
                var WarrantyDetail = await _WarrantyDetailApiService.GetWarrantyDetaiById(WarrantyDetailId);
                if (WarrantyDetail is ApiErrorResult<WarrantyDetailVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (WarrantyDetail.Message != null)
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
                return View(WarrantyDetail.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteWarrantyDetailRequest request)
        {
            try
            {
                var status = await _WarrantyDetailApiService.DeleteWarrantyDetail(request);
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

                return RedirectToAction("Index", "WarrantyDetail");

            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<WarrantyDetailStatus>().ToList();
            ViewBag.ListStatus = statuses;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWarrantyDetailRequest request)
        {
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<WarrantyDetailStatus>().ToList();
            ViewBag.ListStatus = statuses;
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var status = await _WarrantyDetailApiService.CreateWarrantyDetail(request);

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

            TempData["SuccessMsg"] = "Tạo mới thành công cho " + request.WarrantyDetailName;

            return RedirectToAction("Index", "WarrantyDetail");
        }
    }

}
