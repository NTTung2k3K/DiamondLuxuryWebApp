using DiamondLuxurySolution.AdminCrewApp.Service.Material;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class MaterialController : BaseController
    {
        private readonly IMaterialApiService _materialApiService;

        public MaterialController(IMaterialApiService materialApiService)
        {
            _materialApiService = materialApiService;
        }
       

        [HttpGet]
        public async Task<IActionResult> Index(ViewMaterialRequest request)
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

                var material = await _materialApiService.ViewMaterialInManager(request);
                return View(material.ResultObj);
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
        public async Task<IActionResult> Create(CreateMaterialRequest request)
        {

            var status = await _materialApiService.CreateMaterial(request);
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
            TempData["SuccessMsg"] = "Create success for Role " + request.MaterialName;

            return RedirectToAction("Index", "Material");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid MaterialId)
        {
            try
            {
                var material = await _materialApiService.GetMaterialById(MaterialId);
                if (material is ApiErrorResult<MaterialVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (material.Message != null)
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
                return View(material.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateMaterialRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    MaterialVm materialVm = new MaterialVm()
                    {
                        MaterialId = request.MaterialId,
                        Description = request.Description,
                        Status = request.Status
                    };
                    return View(materialVm);
                }
                var status = await _materialApiService.UpdateMaterial(request);
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
                return RedirectToAction("Index", "Material");
            }
            catch
            {
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Detail(Guid MaterialId)
        {
            try
            {
                var status = await _materialApiService.GetMaterialById(MaterialId);
                if (status is ApiErrorResult<MaterialVm> errorResult)
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
        public async Task<IActionResult> Delete(Guid MaterialId)
        {
            try
            {
                var material = await _materialApiService.GetMaterialById(MaterialId);
                if (material is ApiErrorResult<MaterialVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (material.Message != null)
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
                return View(material.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteMaterialRequest request)
        {
            try
            {
                var status = await _materialApiService.DeleteMaterial(request);
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
                return RedirectToAction("Index", "Material");

            }
            catch
            {
                return View();
            }
        }
    }
}
