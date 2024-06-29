using DiamondLuxurySolution.AdminCrewApp.Service.Material;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

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

        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Create(CreateMaterialRequest request)
        {

            var status = await _materialApiService.CreateMaterial(request);
            /*            List<string> listError = new List<string>();
            */
            /*     // Xử lý lỗi từ Require(errorMessage)
                 if (!ModelState.IsValid)
                 {
                     foreach (var modelStateKey in ModelState.Keys)
                     {
                         var modelStateVal = ModelState[modelStateKey];
                         foreach (var error in modelStateVal.Errors)
                         {
                             listError.Add(error.ErrorMessage);
                         }
                     }
                 }

                 // Xử lý lỗi từ API
                 if (status is ApiErrorResult<bool> errorResult)
                 {
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
                 }*/
            
           
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
            return RedirectToAction("Index", "Material");
/*            if (listError.Count == 0)
            {
                TempData["SuccessMsg"] = "Create success for Role " + request.MaterialName;
                return RedirectToAction("Index", "Material");
            }*/
/*            ViewBag.Errors = listError;
            return View();*/
        }
        [Authorize(Roles =  DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

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
						foreach (var error in errorResult.ValidationErrors)
						{
							listError.Add(error);
						}
					}
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
					return View();
				}

				// Chuyển đổi Price từ decimal sang string để xử lý
				string priceString = material.ResultObj.Price.ToString();

				// Cắt bỏ hai số 0 cuối cùng nếu chúng tồn tại
				if (priceString.EndsWith("00"))
				{
					priceString = priceString.Substring(0, priceString.Length - 2);
				}

				// Gán lại giá trị đã xử lý cho Price (nếu cần thiết)
				material.ResultObj.Price = decimal.Parse(priceString);

				return View(material.ResultObj);
			}
			catch
			{
                TempData["ErrorToast"] = true;
                return View();
			}
		}

		[Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateMaterialRequest request)
        {
            try
            {
                var material = await _materialApiService.GetMaterialById(request.MaterialId);

                if (!ModelState.IsValid)
                {
                    MaterialVm materialVm = new MaterialVm()
                    {
                        MaterialId = request.MaterialId,
                        Description = request.Description,
                        Status = request.Status,
                        Color = request.Color,
                        MaterialName = request.MaterialName,
                        Price = material.ResultObj.Price,
                        EffectDate = material.ResultObj.EffectDate,
                        MaterialImage = material.ResultObj.MaterialImage,
                        
                    };
                    TempData["WarningToast"] = true;
                    return View(materialVm);
                }
                var status = await _materialApiService.UpdateMaterial(request);
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
                return RedirectToAction("Index", "Material");
            }
            catch
            {
                TempData["WarningToast"] = true;
                return View();
            }
        }

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

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
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(material.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]

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
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
                    return View();
                }
                TempData["SuccessToast"] = true;
                return RedirectToAction("Index", "Material");

            }
            catch
            {
                TempData["ErrorToast"] = true;
                return View();
            }
        }
    }
}
