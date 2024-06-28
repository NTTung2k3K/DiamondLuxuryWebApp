using DiamondLuxurySolution.AdminCrewApp.Service.Frame;
using DiamondLuxurySolution.AdminCrewApp.Service.Material;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{

    public class FrameController : BaseController
    {
        private readonly IMaterialApiService _materialApiService;
        private readonly IFrameApiService _frameApiService;

        public FrameController(IMaterialApiService materialApiService, IFrameApiService frameApiService)
        {
            _materialApiService = materialApiService;
            _frameApiService = frameApiService;
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Index(ViewFrameRequest request)
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

                var gem = await _frameApiService.ViewFrameInPaging(request);
                return View(gem.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Detail(string FrameId)
        {
            try
            {
                var status = await _frameApiService.GetFrameById(FrameId);
                if (status is ApiErrorResult<FrameVm> errorResult)
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
        public async Task<IActionResult> Delete(string FrameId)
        {
            try
            {
                var frame = await _frameApiService.GetFrameById(FrameId);
                if (frame is ApiErrorResult<FrameVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (frame.Message != null)
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
                return View(frame.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteFrameRequest request)
        {
            try
            {


                var status = await _frameApiService.DeleteFrame(request);
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
                return RedirectToAction("Index", "Frame");

            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Edit(string FrameId)
        {
            try
            {
                var listMaterial = await _materialApiService.GetAll();
                ViewBag.ListMaterial = listMaterial.ResultObj.ToList();

                var frame = await _frameApiService.GetFrameById(FrameId);
                if (frame is ApiErrorResult<FrameVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (frame.Message != null)
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
                return View(frame.ResultObj);
            }
            catch

            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateFrameRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    var listMaterial = await _materialApiService.GetAll();
                    ViewBag.ListMaterial = listMaterial.ResultObj.ToList();
                    var frame = await _frameApiService.GetFrameById(request.FrameId);
                    var material = await _materialApiService.GetMaterialById(frame.ResultObj.MaterialVm.MaterialId);
                    var materialVm = new MaterialVm()
                    {
                        MaterialId = material.ResultObj.MaterialId,
                        Color = material.ResultObj.Color,
                        Description = material.ResultObj.Description,
                        EffectDate = material.ResultObj.EffectDate,
                        MaterialImage = material.ResultObj.MaterialImage,
                        MaterialName = material.ResultObj.MaterialName,
                        Price = material.ResultObj.Price,
                        Status = material.ResultObj.Status,
                    };

                    FrameVm frameVm = new FrameVm()
                    {
                        FrameId = request.FrameId,
                        NameFrame = request.NameFrame,
                        MaterialVm = materialVm,
                        Weight = frame.ResultObj.Weight,
                    };
                    TempData["WarningToast"] = true;
                    return View(frameVm);
                }



                var status = await _frameApiService.UpdateFrame(request);
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
                    var listMaterial = await _materialApiService.GetAll();
                    ViewBag.ListMaterial = listMaterial.ResultObj.ToList();
                    var frame = await _frameApiService.GetFrameById(request.FrameId);
                    var material = await _materialApiService.GetMaterialById(frame.ResultObj.MaterialVm.MaterialId);
                    var materialVm = new MaterialVm()
                    {
                        MaterialId = material.ResultObj.MaterialId,
                        Color = material.ResultObj.Color,
                        Description = material.ResultObj.Description,
                        EffectDate = material.ResultObj.EffectDate,
                        MaterialImage = material.ResultObj.MaterialImage,
                        MaterialName = material.ResultObj.MaterialName,
                        Price = material.ResultObj.Price,
                        Status = material.ResultObj.Status,
                    };

                    FrameVm frameVm = new FrameVm()
                    {
                        FrameId = request.FrameId,
                        NameFrame = request.NameFrame,
                        MaterialVm = materialVm,
                        Weight = frame.ResultObj.Weight,
                    };
                    TempData["WarningToast"] = true;
                    return View(frameVm);

                }
                TempData["SuccessToast"] = true;
                return RedirectToAction("Index", "Frame");
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
            var listMaterial = await _materialApiService.GetAll();
            ViewBag.ListMaterial = listMaterial.ResultObj.ToList();

            return View();
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Create(CreateFrameRequest request)
        {

            var listMaterial = await _materialApiService.GetAll();
            ViewBag.ListMaterial = listMaterial.ResultObj.ToList();

            if (!ModelState.IsValid)
            {
                TempData["WarningToast"] = true;
                return View(request);
            }

            var status = await _frameApiService.CreateFrame(request);

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
            return RedirectToAction("Index", "Frame");
        }




    }
}
