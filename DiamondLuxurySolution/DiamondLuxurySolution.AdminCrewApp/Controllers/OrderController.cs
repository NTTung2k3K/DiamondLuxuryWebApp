using DiamondLuxurySolution.AdminCrewApp.Service.Order;
using DiamondLuxurySolution.AdminCrewApp.Service.Payment;
using DiamondLuxurySolution.AdminCrewApp.Service.Product;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.Application.Repository.Promotion;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using Microsoft.AspNetCore.Mvc;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderApiService _OrderApiService;
        private readonly IStaffApiService _staffApiService;
        private readonly IPromotionApiService _promotionApiService;
        private readonly IPaymentApiService _paymentApiService;
        private readonly IProductApiService _productApiService;

        public OrderController(IOrderApiService OrderApiService,IProductApiService productApiService ,IStaffApiService staffApiService, IPromotionApiService promotionApiService, IPaymentApiService paymentApiService)
        {
            _staffApiService = staffApiService;
            _OrderApiService = OrderApiService;
            _promotionApiService = promotionApiService;
            _paymentApiService = paymentApiService;
            _productApiService= productApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewOrderRequest request)
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

                var Order = await _OrderApiService.ViewOrder(request);
                return View(Order.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string OrderId)
        {
            try
            {
                var status = await _OrderApiService.GetOrderById(OrderId);
                if (status is ApiErrorResult<OrderVm> errorResult)
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
        public async Task<IActionResult> Edit(string OrderId)
        {
            try
            {
                var Order = await _OrderApiService.GetOrderById(OrderId);
                if (Order is ApiErrorResult<OrderVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Order.Message != null)
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
                return View(Order.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateOrderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var OrderVmCall = await _OrderApiService.GetOrderById(request.OrderId);

                    OrderVm OrderVm = new OrderVm()
                    {
                        OrderId = request.OrderId,
                        CustomerVm = OrderVmCall.ResultObj.CustomerVm,
                        DiscountVm = OrderVmCall.ResultObj.DiscountVm,
                        ListOrderProduct = OrderVmCall.ResultObj.ListOrderProduct,
                        ListPromotionVm = OrderVmCall.ResultObj.ListPromotionVm,
                        OrdersPaymentVm = OrderVmCall.ResultObj.OrdersPaymentVm,
                        RemainAmount = OrderVmCall.ResultObj.RemainAmount,
                        ShipAdress = request.ShipAdress,
                        ShipEmail = request.ShipEmail,
                        ShiperVm = OrderVmCall.ResultObj.ShiperVm,
                        ShipName = request.ShipName,
                        ShipPhoneNumber = request.ShipPhoneNumber,
                        Status = request.Status,
                        TotalAmount = OrderVmCall.ResultObj.TotalAmount,
                    };
                    return View(OrderVm);
                }
                var status = await _OrderApiService.UpdateInfoOrder(request);
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
                    return View(request);
                }

                return RedirectToAction("Index", "Order");
            }
            catch
            {
                return View(request);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string OrderId)
        {
            try
            {
                var Order = await _OrderApiService.GetOrderById(OrderId);
                if (Order is ApiErrorResult<OrderVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Order.Message != null)
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
                return View(Order.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string OrderId)
        {
            try
            {
                var status = await _OrderApiService.DeleteOrder(OrderId);
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

                return RedirectToAction("Index", "Order");

            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //Status
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
            ViewBag.ListStatus = statuses;
            var listPayment = await _paymentApiService.GetAll();
            ViewBag.ListPayment = listPayment.ResultObj;
            var listPromotion = await _promotionApiService.GetAllOnTime();
            ViewBag.ListPromotionOnTime = listPromotion.ResultObj;
            var listProduct = await _productApiService.GetAllProduct();
            ViewBag.ListProduct = listProduct.ResultObj;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.USER_ID);
            Guid userId;

            Guid.TryParse(userIdString, out userId);
            // Chuyển đổi thành công
            request.StaffId = userId;
            // Chuyển đổi thành công


            //Status
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
            ViewBag.ListStatus = statuses;
            var listPayment = await _paymentApiService.GetAll();
            ViewBag.ListPayment = listPayment.ResultObj;
            var listPromotion = await _promotionApiService.GetAllOnTime();
            ViewBag.ListPromotionOnTime = listPromotion.ResultObj;
            var listProduct = await _productApiService.GetAll();
            ViewBag.ListProduct = listProduct.ResultObj;


            var status = await _OrderApiService.CreateOrder(request);

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

            TempData["SuccessMsg"] = "Tạo mới thành công cho " + request.ShipName;

            return RedirectToAction("Index", "Order");
        }
    }
}

