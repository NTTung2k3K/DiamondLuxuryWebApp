using DiamondLuxurySolution.AdminCrewApp.Service.Frame;
using DiamondLuxurySolution.AdminCrewApp.Service.Gem;
using DiamondLuxurySolution.AdminCrewApp.Service.GemPriceList;
using DiamondLuxurySolution.AdminCrewApp.Service.IInspectionCertificate;
using DiamondLuxurySolution.AdminCrewApp.Service.Material;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class GemPriceListController : BaseController
    {
        private readonly IGemApiService _gemApiService;
        private readonly IGemPriceListApiService _gemPriceListApiService;
        private readonly IInspectionCertificateApiService _inspectionCertificateApiService;

        public GemPriceListController(IGemApiService gemApiService, IGemPriceListApiService gemPriceListApiService, IInspectionCertificateApiService inspectionCertificateApiService)
        {
            _gemApiService = gemApiService;
            _gemPriceListApiService = gemPriceListApiService;
            _inspectionCertificateApiService = inspectionCertificateApiService;
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Index(ViewGemPriceListRequest request)
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

                var gemPriceList = await _gemPriceListApiService.ViewGemPriceList(request);
                return View(gemPriceList.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Detail(int GemPriceListId)
        {
            try
            {
                var status = await _gemPriceListApiService.GetGemPriceListById(GemPriceListId);
                if (status is ApiErrorResult<GemPriceListVm> errorResult)
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
        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Delete(int GemPriceListId)
        {
            try
            {
                var gemPriceList = await _gemPriceListApiService.GetGemPriceListById(GemPriceListId);
                if (gemPriceList is ApiErrorResult<GemPriceListVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (gemPriceList.Message != null)
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
                return View(gemPriceList.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]


        [HttpPost]
        public async Task<IActionResult> Delete(DeleteGemPriceListRequest request)
        {
            try
            {


                var status = await _gemPriceListApiService.DeleteGemPriceList(request);
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
                    ViewBag.Errors = listError;
                    return View();

                }
                return RedirectToAction("Index", "GemPriceList");

            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Edit(int GemPriceListId)
        {
            try
            {
                var listGem = await _gemApiService.GetAll();
                ViewBag.ListGem = listGem.ResultObj.ToList();

                var gemPriceList = await _gemPriceListApiService.GetGemPriceListById(GemPriceListId);
                if (gemPriceList is ApiErrorResult<GemPriceListVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (gemPriceList.Message != null)
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
                return View(gemPriceList.ResultObj);
            }
            catch

            {
                return View();
            }
        }
        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateGemPriceListRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
					var listGem = await _gemApiService.GetAll();
					ViewBag.ListGem = listGem.ResultObj.ToList();

					var gemPriceList = await _gemPriceListApiService.GetGemPriceListById(request.GemPriceListId);
                    var gem = await _gemApiService.GetGemById(gemPriceList.ResultObj.GemVm.GemId);
                    var inspectionCertificate = await _inspectionCertificateApiService.GetInspectionCertificateById(gem.ResultObj.InspectionCertificateVm.InspectionCertificateId);
                    var inspectionCertificateVm = new InspectionCertificateVm()
                    {
                        InspectionCertificateId = inspectionCertificate.ResultObj.InspectionCertificateId,
                        InspectionCertificateName = inspectionCertificate.ResultObj.InspectionCertificateName,
                        DateGrading = inspectionCertificate.ResultObj?.DateGrading,
                        Logo = inspectionCertificate.ResultObj?.Logo,
                        Status = inspectionCertificate.ResultObj.Status,
                    };

                    var gemVm = new GemVm()
                    {
                        GemId = gem.ResultObj.GemId,
                        GemName = gem.ResultObj.GemName,
                        GemImage = gem.ResultObj.GemImage,
                        ProportionImage = gem.ResultObj.ProportionImage,
                        Polish = gem.ResultObj.Polish,
                        Symetry = gem.ResultObj.Symetry,
                        AcquisitionDate = gem.ResultObj.AcquisitionDate,
                        Active = gem.ResultObj.Active,
                        Fluoresence = gem.ResultObj.Fluoresence,
                        IsOrigin = gem.ResultObj.IsOrigin,
                        InspectionCertificateVm = inspectionCertificateVm,
                    };

                    var gemPriceListVm = new GemPriceListVm()
                    {
                        GemPriceListId = gemPriceList.ResultObj.GemPriceListId,
                        Active = gemPriceList.ResultObj.Active,
                        GemVm = gemVm,
                    };
                    return View(gemPriceListVm);
                }

                

                var status = await _gemPriceListApiService.UpdateGemPriceList(request);
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
					var listGem = await _gemApiService.GetAll();
					ViewBag.ListGem = listGem.ResultObj.ToList();

					var gemPriceList = await _gemPriceListApiService.GetGemPriceListById(request.GemPriceListId);
                    var gem = await _gemApiService.GetGemById(gemPriceList.ResultObj.GemVm.GemId);
                    var inspectionCertificate = await _inspectionCertificateApiService.GetInspectionCertificateById(gem.ResultObj.InspectionCertificateVm.InspectionCertificateId);
                    var inspectionCertificateVm = new InspectionCertificateVm()
                    {
                        InspectionCertificateId = inspectionCertificate.ResultObj.InspectionCertificateId,
                        InspectionCertificateName = inspectionCertificate.ResultObj.InspectionCertificateName,
                        DateGrading = inspectionCertificate.ResultObj?.DateGrading,
                        Logo = inspectionCertificate.ResultObj?.Logo,
                        Status = inspectionCertificate.ResultObj.Status,
                    };

                    var gemVm = new GemVm()
                    {
                        GemId = gem.ResultObj.GemId,
                        GemName = gem.ResultObj.GemName,
                        GemImage = gem.ResultObj.GemImage,
                        ProportionImage = gem.ResultObj.ProportionImage,
                        Polish = gem.ResultObj.Polish,
                        Symetry = gem.ResultObj.Symetry,
                        AcquisitionDate = gem.ResultObj.AcquisitionDate,
                        Active = gem.ResultObj.Active,
                        Fluoresence = gem.ResultObj.Fluoresence,
                        IsOrigin = gem.ResultObj.IsOrigin,
                        InspectionCertificateVm = inspectionCertificateVm,
                    };

                    var gemPriceListVm = new GemPriceListVm()
                    {
                        GemPriceListId = gemPriceList.ResultObj.GemPriceListId,
                        Active = gemPriceList.ResultObj.Active,
                        GemVm = gemVm,
                    };
                    return View(gemPriceListVm);

                }
                return RedirectToAction("Index", "GemPriceList");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
		public async Task<IActionResult> Create()
		{
			var listGem = await _gemApiService.GetAll();
			ViewBag.ListGem = listGem.ResultObj.ToList();

			return View();
		}
        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
		public async Task<IActionResult> Create(CreateGemPriceListRequest request)
		    {

			var listGem = await _gemApiService.GetAll();
			ViewBag.ListGem = listGem.ResultObj.ToList();

			if (!ModelState.IsValid)
			{

				return View(request);
			}

			var status = await _gemPriceListApiService.CreateGemPriceList(request);

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
			TempData["SuccessMsg"] = "Create success for Role " + request.CaratWeight;

			return RedirectToAction("Index", "GemPriceList");
		}
	}
}
