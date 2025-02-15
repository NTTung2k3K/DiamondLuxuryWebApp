﻿using DiamondLuxurySolution.AdminCrewApp.Service.Payment;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentApiService _paymentApiService;

        public PaymentController(IPaymentApiService paymentApiService)
        {
            _paymentApiService = paymentApiService;
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Index(ViewPaymentRequest request)
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

                var Payment = await _paymentApiService.ViewInPayment(request);
                return View(Payment.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Detail(Guid PaymentId)
        {
            try
            {
                var status = await _paymentApiService.GetPaymentById(PaymentId);
                if (status is ApiErrorResult<PaymentVm> errorResult)
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
        public async Task<IActionResult> Edit(Guid PaymentId)
        {
            try
            {
                var Payment = await _paymentApiService.GetPaymentById(PaymentId);
                if (Payment is ApiErrorResult<PaymentVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Payment.Message != null)
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
                return View(Payment.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Edit(UpdatePaymentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PaymentVm paymentVm = new PaymentVm()
                    {
                        PaymentMethod = request.PaymentMethod,
                        Description = request.Description,
                        Status = request.Status,
                        PaymentId = request.PaymentId,
                    };
                    TempData["WarningToast"] = true;
                    return View(paymentVm);
                }

                var status = await _paymentApiService.UpdatePayment(request);
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
                return RedirectToAction("Index", "Payment");
            }
            catch
            {
                TempData["WarningToast"] = true;
                return View();
            }
        }



        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Delete(Guid PaymentId)
        {
            try
            {
                var Payment = await _paymentApiService.GetPaymentById(PaymentId);
                if (Payment is ApiErrorResult<PaymentVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Payment.Message != null)
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
                return View(Payment.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Delete(DeletePaymentRequest request)
        {
            try
            {
                if (request == null)
                {
                    TempData["ErrorToast"] = true;
                    return View(request);
                }

                var status = await _paymentApiService.DeletePayment(request);

                if (status is ApiErrorResult<bool> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (status.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    else if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in errorResult.ValidationErrors)
                        {
                            listError.Add(error);
                        }
                    }
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
                    return View();
                }
                TempData["SuccessToast"] = true;
                return RedirectToAction("Index", "Payment");
            }
            catch
            {
                TempData["ErrorToast"] = true;
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
        public async Task<IActionResult> Create(CreatePaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["WarningToast"] = true;
                return View(request);
            }

            var status = await _paymentApiService.CreatePayment(request);

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

            return RedirectToAction("Index", "Payment");
        }
    }
}
