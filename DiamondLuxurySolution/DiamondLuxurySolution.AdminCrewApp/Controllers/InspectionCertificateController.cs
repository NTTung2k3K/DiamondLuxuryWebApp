using DiamondLuxurySolution.AdminCrewApp.Service.IInspectionCertificate;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
/*    [Route("Catalog/[InspectionCertificate]")]
*/    
    
    
    public class InspectionCertificateController : BaseController
    {
        private readonly IInspectionCertificateApiService _inspectionCertificateApiService;

        public InspectionCertificateController(IInspectionCertificateApiService inspectionCertificateApiService)
        {
            _inspectionCertificateApiService = inspectionCertificateApiService;
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Index(ViewInspectionCertificateRequest request)
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
        
                var inspectionCertificate = await _inspectionCertificateApiService.ViewInspectionCertificateInManager(request);
                return View(inspectionCertificate.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Detail(string InspectionCertificateId)
        {
            try
            {
                var status = await _inspectionCertificateApiService.GetInspectionCertificateById(InspectionCertificateId);
                if (status is ApiErrorResult<InspectionCertificateVm> errorResult)
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
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Edit(string InspectionCertificateId)
        {
            try
            {
                var inspectionCertificate = await _inspectionCertificateApiService.GetInspectionCertificateById(InspectionCertificateId);
                if (inspectionCertificate is ApiErrorResult<InspectionCertificateVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (inspectionCertificate.Message != null)
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
                return View(inspectionCertificate.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateInspectionCertificateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    InspectionCertificateVm inspectionCertificateVm = new InspectionCertificateVm()
                    {
                        InspectionCertificateName = request.InspectionCertificateName,
                        DateGrading = request.DateGrading,
                        InspectionCertificateId = request.InspectionCertificateId,
                        Status = request.Status
                    };
                    return View(inspectionCertificateVm);
                }
                var status = await _inspectionCertificateApiService.UpdateInspectionCertificate(request);
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
                return RedirectToAction("Index", "InspectionCertificate");
            }
            catch
            {
                return View();
            }
        }


        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Delete(string InspectionCertificateId)
        {
            try
            {
                var inspectionCertificate = await _inspectionCertificateApiService.GetInspectionCertificateById(InspectionCertificateId);
                if (inspectionCertificate is ApiErrorResult<InspectionCertificateVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (inspectionCertificate.Message != null)
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
                return View(inspectionCertificate.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteInspectionCertificateRequest request)
        {
            try
            {
                var status = await _inspectionCertificateApiService.DeleteInspectionCertificate(request);
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
                return RedirectToAction("Index", "InspectionCertificate");

            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Create(CreateInspectionCertificateRequest request)
        {


            var status = await _inspectionCertificateApiService.CreateInspectionCertificate(request);

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
            TempData["SuccessMsg"] = "Create success for Role " + request.InspectionCertificateName;

            return RedirectToAction("Index", "InspectionCertificate");
        }


    }
}

