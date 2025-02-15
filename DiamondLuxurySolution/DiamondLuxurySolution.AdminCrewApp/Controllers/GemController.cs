﻿using DiamondLuxurySolution.AdminCrewApp.Service.Contact;
using DiamondLuxurySolution.AdminCrewApp.Service.Gem;
using DiamondLuxurySolution.AdminCrewApp.Service.GemPriceList;
using DiamondLuxurySolution.AdminCrewApp.Service.IInspectionCertificate;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class GemController : BaseController
    {
        private readonly IInspectionCertificateApiService _inspectionCertificateApiService;
        private readonly IGemPriceListApiService _gemPriceListApiService;
        private readonly IGemApiService _gemApiService;

        public GemController(IInspectionCertificateApiService inspectionCertificateApiService, IGemApiService gemApiService, IGemPriceListApiService gemPriceListApiService)
        {
            _inspectionCertificateApiService = inspectionCertificateApiService;
            _gemPriceListApiService = gemPriceListApiService;
            _gemApiService = gemApiService;
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Index(ViewGemRequest request)
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

                var gem = await _gemApiService.ViewGemInManager(request);
                return View(gem.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Edit(Guid GemId)
        {
            try
            {
                var listGemPriceList = await _gemPriceListApiService.GetAll();
                ViewBag.listGemPriceList = listGemPriceList.ResultObj.ToList();

                var gem = await _gemApiService.GetGemById(GemId);
                if (gem is ApiErrorResult<GemVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (gem.Message != null)
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
                return View(gem.ResultObj);
            }
            catch

            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateGemResquest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var listGemPriceList = await _gemPriceListApiService.GetAll();
                    ViewBag.listGemPriceList = listGemPriceList.ResultObj.ToList();
                    var gem = await _gemApiService.GetGemById(request.GemId);
                    var insp = await _inspectionCertificateApiService.GetInspectionCertificateById(gem.ResultObj.InspectionCertificateVm.InspectionCertificateId);
                    var inspectionCertificateVm = new InspectionCertificateVm()
                    {
                        InspectionCertificateId = insp.ResultObj.InspectionCertificateId,
                        InspectionCertificateName = insp.ResultObj.InspectionCertificateName,
                        DateGrading = insp.ResultObj.DateGrading,
                        Logo = insp.ResultObj.Logo,
                        Status = insp.ResultObj.Status,
                    };
                    var gpl = await _gemPriceListApiService.GetGemPriceListById(gem.ResultObj.GemPriceListVm.GemPriceListId);
                    var gplVm = new GemPriceListVm()
                    {
                        GemPriceListId = gpl.ResultObj.GemPriceListId,
                        Active = gpl.ResultObj.Active,
                        CaratWeight = gpl.ResultObj.CaratWeight,
                        Clarity = gpl.ResultObj.Clarity,
                        Color = gpl.ResultObj.Color,
                        Cut = gpl.ResultObj.Cut,
                        effectDate = gpl.ResultObj.effectDate,
                        Price = gpl.ResultObj.Price,
                    };

                    GemVm gemVm = new GemVm()
                    {
                        GemId = request.GemId,
                        GemName = request.GemName,
                        Symetry = request.Symetry != null ? request.Symetry : null,
                        Polish = request.Polish != null ? request.Polish : null,
                        IsOrigin = request.IsOrigin,
                        Fluoresence = request.Fluoresence,
                        Active = request.Active,
                        InspectionCertificateVm = inspectionCertificateVm,
                        GemPriceListVm = gplVm,
                        AcquisitionDate = gem.ResultObj.AcquisitionDate,
                        GemImage = gem.ResultObj.GemImage,
                        ProportionImage = gem.ResultObj.ProportionImage,

                    };
                    TempData["WarningToast"] = true;
                    return View(gemVm);
                }
                var status = await _gemApiService.UpdateGem(request);
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
                return RedirectToAction("Index", "Gem");
            }
            catch
            {
                TempData["WarningToast"] = true;
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Detail(Guid GemId)
        {
            try
            {
                var status = await _gemApiService.GetGemById(GemId);
                if (status is ApiErrorResult<GemVm> errorResult)
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
        public async Task<IActionResult> Delete(Guid GemId)
        {
            try
            {
                var gem = await _gemApiService.GetGemById(GemId);
                if (gem is ApiErrorResult<GemVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (gem.Message != null)
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
                return View(gem.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteGemRequest request)
        {
            try
            {


                var status = await _gemApiService.DeleteGem(request);
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
                return RedirectToAction("Index", "Gem");

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
            var listInsp = await _inspectionCertificateApiService.GetAll();
            var listGem = await _gemApiService.GetAll();

            // Lấy tất cả các InspectionCertificateId có trong listGem
            var gemInspectionCertificateIds = listGem.ResultObj
                                                     .Select(gem => gem.InspectionCertificateVm.InspectionCertificateId)
                                                     .ToList();

            // Lấy các InspectionCertificateId chưa có trong gem
            var availableInspectionCertificates = listInsp.ResultObj
                              .Where(insp => !gemInspectionCertificateIds.Any(gemId => gemId.Equals(insp.InspectionCertificateId)))
                              .ToList();

            ViewBag.ListIsnp = availableInspectionCertificates;

            var listGemPriceList = await _gemPriceListApiService.GetAll();
            ViewBag.listGemPriceList = listGemPriceList.ResultObj.ToList();

            return View();
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Create(CreateGemRequest request)
        {
            var listInsp = await _inspectionCertificateApiService.GetAll();
            var listGem = await _gemApiService.GetAll();

            // Lấy tất cả các InspectionCertificateId có trong listGem
            var gemInspectionCertificateIds = listGem.ResultObj
                                                     .Select(gem => gem.InspectionCertificateVm.InspectionCertificateId)
                                                     .ToList();

            // Lấy các InspectionCertificateId chưa có trong gem
            var availableInspectionCertificates = listInsp.ResultObj
                              .Where(insp => !gemInspectionCertificateIds.Any(gemId => gemId.Equals(insp.InspectionCertificateId)))
                              .ToList();

            ViewBag.ListIsnp = availableInspectionCertificates;

            var listGemPriceList = await _gemPriceListApiService.GetAll();
            ViewBag.listGemPriceList = listGemPriceList.ResultObj.ToList();

            if (!ModelState.IsValid)
            {
                TempData["WarningToast"] = true;
                return View(request);
            }

            var status = await _gemApiService.CreateGem(request);

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

                ViewBag.ListIsnp = availableInspectionCertificates;

                return View();

            }
            TempData["SuccessToast"] = true;

            return RedirectToAction("Index", "Gem");
        }
    }
}
