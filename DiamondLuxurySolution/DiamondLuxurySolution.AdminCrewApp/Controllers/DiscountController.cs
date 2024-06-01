using DiamondLuxurySolution.AdminCrewApp.Service.Discount;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Discount;
using DiamondLuxurySolution.ViewModel.Models.Role;
using Microsoft.AspNetCore.Mvc;
namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountApiService _discountApiService;

        public DiscountController(IDiscountApiService discountApiService)
        {
            _discountApiService = discountApiService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(ViewDiscountRequest request)
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

                var discount = await _discountApiService.ViewDiscountInManager(request);
                return View(discount.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string DiscountId)
        {
            try
            {
                var status = await _discountApiService.GetDiscountById(DiscountId);
                if (status is ApiErrorResult<DiscountVm> errorResult)
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
        public async Task<IActionResult> Edit(string DiscountId)
        {
            try
            {
                var discount = await _discountApiService.GetDiscountById(DiscountId);
                if (discount is ApiErrorResult<DiscountVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (discount.Message != null)
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
                return View(discount.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateDiscountRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    DiscountVm discountVm = new DiscountVm()
                    {
                        Description = request.Description,
                        DiscountId = request.DiscountId,
                        DiscountName = request.DiscountName,
                        PercentSale = Convert.ToDouble(request.PercentSale),
                        Status = request.Status,
                    };
                    return View(discountVm);
                }

                var status = await _discountApiService.UpdateDiscount(request);
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
				return RedirectToAction("Index", "Discount");
            }
            catch
            {
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string DiscountId)
        {
            try
            {
                var discount = await _discountApiService.GetDiscountById(DiscountId);
                if (discount is ApiErrorResult<DiscountVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (discount.Message != null)
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
                return View(discount.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteDiscountRequest request)
        {
            try
            {


                var status = await _discountApiService.DeleteDiscount(request);
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
                return RedirectToAction("Index", "Discount");

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
        public async Task<IActionResult> Create(CreateDiscountRequest request)
        {


            var status = await _discountApiService.CreateDiscount(request);

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
			TempData["SuccessMsg"] = "Tạo thành công mã giảm giá " + request.DiscountName;

            return RedirectToAction("Index", "Discount");
        }


    }

}
