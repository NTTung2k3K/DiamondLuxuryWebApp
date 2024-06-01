using DiamondLuxurySolution.AdminCrewApp.Service.Contact;
using DiamondLuxurySolution.AdminCrewApp.Service.Gem;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class GemController : Controller
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IGemApiService _gemApiService;

        public GemController(LuxuryDiamondShopContext context , IGemApiService gemApiService)
        {
            _context = context;
            _gemApiService = gemApiService;
        }

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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid GemId)
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
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(gem.ResultObj);
            }
            catch

            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateGemResquest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var gem = await _context.Gems.FindAsync(request.GemId);
                    var insp = await _context.InspectionCertificates.FindAsync(gem.InspectionCertificateId);
                    var inspectionCertificateVm = new InspectionCertificateVm()
                    {
                        InspectionCertificateId = insp.InspectionCertificateId,
                        InspectionCertificateName = insp.InspectionCertificateName,
                        DateGrading = insp.DateGrading,
                        Logo = insp.Logo,
                        Status = insp.Status,
                    };

                    GemVm gemVm = new GemVm()
                    {
                        GemId = request.GemId,
                        GemName = request.GemName,
                        GemImage = request.GemImage.ToString(),
                        Symetry = request.Symetry,
                        Polish = request.Polish,
                        ProportionImage = request.ProportionImage.ToString(),
                        IsOrigin = request.IsOrigin,
                        Fluoresence = request.Fluoresence,
                        AcquisitionDate = (DateTime)request.AcquisitionDate,
                        Active = request.Active,
                        InspectionCertificateVm = inspectionCertificateVm

                    };
                    return View(gemVm);
                }
                var status = await _gemApiService.UpdateGem(request);
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
                return RedirectToAction("Index", "Gem");
            }
            catch
            {
                return View();
            }
        }

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
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(gem.ResultObj);
            }
            catch
            {
                return View();
            }
        }

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
                    ViewBag.Errors = listError;
                    return View();

                }
                return RedirectToAction("Index", "Gem");

            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateGemRequest request)
        {


            var status = await _gemApiService.CreateGem(request);

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
            TempData["SuccessMsg"] = "Create success for Role " + request.GemName;

            return RedirectToAction("Index", "Gem");
        }
    }
}
