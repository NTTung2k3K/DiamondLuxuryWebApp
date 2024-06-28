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
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(gemPriceList.ResultObj);
            }
            catch
            {
                TempData["ErrorToast"] = true;
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
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
                    return View();

                }
                TempData["SuccessToast"] = true;
                return RedirectToAction("Index", "GemPriceList");

            }
            catch
            {
                TempData["ErrorToast"] = true;
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
                    TempData["ErrorToast"] = true;
                    ViewBag.Errors = listError;
					return View();
				}

				// Chuyển đổi Price từ decimal sang string để xử lý
				string priceString = gemPriceList.ResultObj.Price.ToString("0.##");

				// Gán lại giá trị đã xử lý cho Price (nếu cần thiết)
				gemPriceList.ResultObj.Price = decimal.Parse(priceString);

				return View(gemPriceList.ResultObj);
			}
			catch
			{
                TempData["ErrorToast"] = true;
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

                    var gemPriceListVm = new GemPriceListVm()
                    {
                        GemPriceListId = gemPriceList.ResultObj.GemPriceListId,
                        Active = gemPriceList.ResultObj.Active,
                        CaratWeight = gemPriceList.ResultObj.CaratWeight,
                        Clarity = gemPriceList.ResultObj.Clarity,
                        Color = gemPriceList.ResultObj.Color,
                        Cut = gemPriceList.ResultObj.Cut,
                        effectDate = gemPriceList.ResultObj.effectDate,
                        Price = gemPriceList.ResultObj.Price,
                    };
                    TempData["WarningToast"] = true;
                    return View(gemPriceListVm);
                }

                // Chuyển đổi giá trị Price từ định dạng chuỗi có dấu phân cách hàng nghìn về số nguyên
                request.Price = decimal.Parse(request.Price.Replace(".", "").Replace(",", "")).ToString();

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

                    var gemPriceListVm = new GemPriceListVm()
                    {
                        GemPriceListId = gemPriceList.ResultObj.GemPriceListId,
                        Active = gemPriceList.ResultObj.Active,
                        CaratWeight = gemPriceList.ResultObj.CaratWeight,
                        Clarity = gemPriceList.ResultObj.Clarity,
                        Color = gemPriceList.ResultObj.Color,
                        Cut = gemPriceList.ResultObj.Cut,
                        effectDate = gemPriceList.ResultObj.effectDate,
                        Price = gemPriceList.ResultObj.Price,
                    };
                    TempData["WarningToast"] = true;
                    return View(gemPriceListVm);
                }
                TempData["SuccessToast"] = true;
                return RedirectToAction("Index", "GemPriceList");
            }
            catch
            {
                TempData["WarningToast"] = true;
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
                TempData["WarningToast"] = true;
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
                TempData["WarningToast"] = true;
                ViewBag.Errors = listError;
				return View();

			}
            TempData["SuccessToast"] = true;

            return RedirectToAction("Index", "GemPriceList");
		}
	}
}
