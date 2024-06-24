using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Service.Order;
using DiamondLuxurySolution.AdminCrewApp.Service.Payment;
using DiamondLuxurySolution.AdminCrewApp.Service.Product;
using DiamondLuxurySolution.AdminCrewApp.Service.Promotion;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.Application.Repository.Promotion;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using System.Runtime.InteropServices;
using System.Text.Json;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;
using Rotativa;
using HtmlToPdfMaster;
using Microsoft.VisualBasic.FileIO;
using PdfSharp;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderApiService _OrderApiService;
        private readonly IStaffApiService _staffApiService;
        private readonly IPromotionApiService _promotionApiService;
        private readonly IPaymentApiService _paymentApiService;
        private readonly IProductApiService _productApiService;
        private readonly IConverter _converter;


        public OrderController(IConverter converter, IOrderApiService OrderApiService, IProductApiService productApiService, IStaffApiService staffApiService, IPromotionApiService promotionApiService, IPaymentApiService paymentApiService)
        {
            _staffApiService = staffApiService;
            _OrderApiService = OrderApiService;
            _promotionApiService = promotionApiService;
            _paymentApiService = paymentApiService;
            _productApiService = productApiService;
            _converter = converter;
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]
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
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff + ", " + DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]
        [HttpGet]
        public async Task<IActionResult> Detail(string OrderId)
        {
            try
            {
                var Order = await _OrderApiService.GetOrderById(OrderId);
                ViewBag.PaymentList = Order.ResultObj.OrdersPaymentVm;

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

        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpGet]
        public async Task<IActionResult> Edit(string OrderId)
        {
            try
            {
                var transactionStatus = Enum.GetValues(typeof(TransactionStatus)).Cast<TransactionStatus>().ToList();
                ViewBag.TransactionStatus = transactionStatus;

                //Status
                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
                ViewBag.ListStatus = statuses;
                var listPayment = await _paymentApiService.GetAll();
                ViewBag.ListPayment = listPayment.ResultObj;
                var listPromotion = await _promotionApiService.GetAllOnTime();
                ViewBag.ListPromotionOnTime = listPromotion.ResultObj;
                var listProduct = await _productApiService.GetAllProduct();
                ViewBag.ListProduct = listProduct.ResultObj;
                var Order = await _OrderApiService.GetOrderById(OrderId);
                ViewBag.Total = Order.ResultObj.TotalAmount;
                ViewBag.TotalSale = Order.ResultObj.TotalSale;
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
                List<OrderProductSupport> listExistProduct = new List<OrderProductSupport>();
                foreach (var item in Order.ResultObj.ListOrderProduct)
                {
                    var existProduct = new OrderProductSupport()
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Size = item.Size
                    };
                    listExistProduct.Add(existProduct);
                }
                List<Guid> listPaymentExist = new List<Guid>();
                foreach (var item in Order.ResultObj.OrdersPaymentVm)
                {

                    listPaymentExist.Add(item.PaymentId);
                }

                ViewBag.PaymentList = Order.ResultObj.OrdersPaymentVm;

                var updateVm = new UpdateOrderRequest()
                {
                    OrderId = Order.ResultObj.OrderId,
                    Status = Order.ResultObj.Status.ToString(),
                    CustomerId = Order.ResultObj.CustomerVm.CustomerId,
                    Deposit = Order.ResultObj.Deposit,
                    Description = Order.ResultObj.Description,
                    DiscountId = Order.ResultObj.DiscountVm == null ? null : Order.ResultObj.DiscountVm.DiscountId,
                    Email = Order.ResultObj.ShipEmail,
                    Fullname = Order.ResultObj.CustomerVm.FullName,
                    StaffId = Order.ResultObj.StaffVm == null ? null : Order.ResultObj.StaffVm.StaffId,
                    ListExistOrderProduct = listExistProduct,
                    PhoneNumber = Order.ResultObj.ShipPhoneNumber,
                    PromotionId = Order.ResultObj.PromotionVm == null ? null : Order.ResultObj.PromotionVm.PromotionId,
                    ShipAdress = Order.ResultObj.ShipAdress,
                    ListPaymentId = listPaymentExist,
                };
                return View(updateVm);
            }
            catch
            {
                return View();
            }
        }



        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateOrderRequest request)
        {
            try
            {
                var listOrder = new List<OrderProductSupport>();
                foreach (var item in request.ListExistOrderProduct)
                {
                    if (item.Quantity == 0)
                    {
                        continue;
                    }
                    listOrder.Add(item);
                }
                request.ListExistOrderProduct = listOrder;
                var transactionStatus = Enum.GetValues(typeof(TransactionStatus)).Cast<TransactionStatus>().ToList();
                ViewBag.TransactionStatus = transactionStatus;

                //Status
                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
                ViewBag.ListStatus = statuses;
                var listPayment = await _paymentApiService.GetAll();
                ViewBag.ListPayment = listPayment.ResultObj;
                var listPromotion = await _promotionApiService.GetAllOnTime();
                ViewBag.ListPromotionOnTime = listPromotion.ResultObj;
                var listProduct = await _productApiService.GetAllProduct();
                ViewBag.ListProduct = listProduct.ResultObj;

                var Order = await _OrderApiService.GetOrderById(request.OrderId);
                ViewBag.Total = Order.ResultObj.TotalAmount;
                ViewBag.TotalSale = Order.ResultObj.TotalSale;


                string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.USER_ID);
                Guid userId;

                Guid.TryParse(userIdString, out userId);
                // Chuyển đổi thành công
                request.StaffId = userId;
                if (request.StatusOrderPayment != null && request.StatusOrderPayment.Count > 0)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // If needed
                        WriteIndented = true // If neededf
                    };
                    string listSubGemsJson = System.Text.Json.JsonSerializer.Serialize(request.StatusOrderPayment, options);
                    request.StatusOrderPaymentJson = listSubGemsJson;
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
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpGet]
        public async Task<IActionResult> PaidTheRest(string OrderId)
        {
            try
            {
                var transactionStatus = Enum.GetValues(typeof(TransactionStatus)).Cast<TransactionStatus>().ToList();
                ViewBag.TransactionStatus = transactionStatus;

                //Status
                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
                ViewBag.ListStatus = statuses;
                var listPayment = await _paymentApiService.GetAll();
                ViewBag.ListPayment = listPayment.ResultObj;
                var listPromotion = await _promotionApiService.GetAllOnTime();
                ViewBag.ListPromotionOnTime = listPromotion.ResultObj;
                var listProduct = await _productApiService.GetAllProduct();
                ViewBag.ListProduct = listProduct.ResultObj;
                ViewBag.PaymentId = "";
                ViewBag.PaidTheRest = 0;
                ViewBag.Message = "";
                var Order = await _OrderApiService.GetOrderById(OrderId);
                ViewBag.Total = Order.ResultObj.TotalAmount;
                ViewBag.TotalSale = Order.ResultObj.TotalSale;
                ViewBag.TransStatus = "";


                List<OrderProductSupport> listExistProduct = new List<OrderProductSupport>();
                foreach (var item in Order.ResultObj.ListOrderProduct)
                {
                    var existProduct = new OrderProductSupport()
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    };
                    listExistProduct.Add(existProduct);
                }
                List<Guid> listPaymentExist = new List<Guid>();
                foreach (var item in Order.ResultObj.OrdersPaymentVm)
                {

                    listPaymentExist.Add(item.PaymentId);
                }

                ViewBag.PaymentList = Order.ResultObj.OrdersPaymentVm;

                var updateVm = new UpdateOrderRequest()
                {
                    OrderId = Order.ResultObj.OrderId,
                    Status = Order.ResultObj.Status.ToString(),
                    CustomerId = Order.ResultObj.CustomerVm.CustomerId,
                    Deposit = Order.ResultObj.Deposit,
                    Description = Order.ResultObj.Description,
                    DiscountId = Order.ResultObj.DiscountVm == null ? null : Order.ResultObj.DiscountVm.DiscountId,
                    Email = Order.ResultObj.ShipEmail,
                    Fullname = Order.ResultObj.CustomerVm.FullName,
                    StaffId = Order.ResultObj.StaffVm.StaffId,
                    ListExistOrderProduct = listExistProduct,
                    PhoneNumber = Order.ResultObj.ShipPhoneNumber,
                    PromotionId = Order.ResultObj.PromotionVm == null ? null : Order.ResultObj.PromotionVm.PromotionId,
                    ShipAdress = Order.ResultObj.ShipAdress,
                    ListPaymentId = listPaymentExist,

                };
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
                    return View(updateVm);

                }
                return View(updateVm);
            }
            catch
            {
                return View();
            }
        }



        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpPost]
        public async Task<IActionResult> PaidTheRest(ContinuePaymentRequest request)
        {
            try
            {
                var transactionStatus = Enum.GetValues(typeof(TransactionStatus)).Cast<TransactionStatus>().ToList();
                ViewBag.TransactionStatus = transactionStatus;

                //Status
                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
                ViewBag.ListStatus = statuses;
                var listPayment = await _paymentApiService.GetAll();
                ViewBag.ListPayment = listPayment.ResultObj;
                var listPromotion = await _promotionApiService.GetAllOnTime();
                ViewBag.ListPromotionOnTime = listPromotion.ResultObj;
                var listProduct = await _productApiService.GetAllProduct();
                ViewBag.ListProduct = listProduct.ResultObj;
                ViewBag.PaymentId = request.PaymentId.ToString();
                ViewBag.PaidTheRest = request.PaidTheRest;
                ViewBag.Message = request.Message;
                ViewBag.TransStatus = request.TransactionStatus;


                string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.USER_ID);
                Guid userId;

                Guid.TryParse(userIdString, out userId);
                var status = await _OrderApiService.ContinuePayment(request);
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

                    var Order = await _OrderApiService.GetOrderById(request.OrderId);
                    ViewBag.Total = Order.ResultObj.TotalAmount;
                    ViewBag.TotalSale = Order.ResultObj.TotalSale;

                    List<OrderProductSupport> listExistProduct = new List<OrderProductSupport>();
                    foreach (var item in Order.ResultObj.ListOrderProduct)
                    {
                        var existProduct = new OrderProductSupport()
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                        };
                        listExistProduct.Add(existProduct);
                    }
                    List<Guid> listPaymentExist = new List<Guid>();
                    foreach (var item in Order.ResultObj.OrdersPaymentVm)
                    {

                        listPaymentExist.Add(item.PaymentId);
                    }

                    ViewBag.PaymentList = Order.ResultObj.OrdersPaymentVm;

                    var updateVm = new UpdateOrderRequest()
                    {
                        OrderId = Order.ResultObj.OrderId,
                        Status = Order.ResultObj.Status.ToString(),
                        CustomerId = Order.ResultObj.CustomerVm.CustomerId,
                        Deposit = Order.ResultObj.Deposit,
                        Description = Order.ResultObj.Description,
                        DiscountId = Order.ResultObj.DiscountVm == null ? null : Order.ResultObj.DiscountVm.DiscountId,
                        Email = Order.ResultObj.ShipEmail,
                        Fullname = Order.ResultObj.CustomerVm.FullName,
                        StaffId = Order.ResultObj.StaffVm.StaffId,
                        ListExistOrderProduct = listExistProduct,
                        PhoneNumber = Order.ResultObj.ShipPhoneNumber,
                        PromotionId = Order.ResultObj.PromotionVm == null ? null : Order.ResultObj.PromotionVm.PromotionId,
                        ShipAdress = Order.ResultObj.ShipAdress,
                        ListPaymentId = listPaymentExist,
                    };


                    return View(updateVm);
                }

                return RedirectToAction("Index", "Order");
            }
            catch
            {
                return View(request);
            }
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

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
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

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
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var transactionStatus = Enum.GetValues(typeof(TransactionStatus)).Cast<TransactionStatus>().ToList();
            ViewBag.TransactionStatus = transactionStatus;

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
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff)]

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderByStaffRequest request)
        {
            //Status
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
            var transactionStatus = Enum.GetValues(typeof(TransactionStatus)).Cast<TransactionStatus>().ToList();
            ViewBag.TransactionStatus = transactionStatus;

            ViewBag.ListStatus = statuses;
            var listPayment = await _paymentApiService.GetAll();
            ViewBag.ListPayment = listPayment.ResultObj;
            var listPromotion = await _promotionApiService.GetAllOnTime();
            ViewBag.ListPromotionOnTime = listPromotion.ResultObj;
            var listProduct = await _productApiService.GetAllProduct();
            ViewBag.ListProduct = listProduct.ResultObj;

            if (!ModelState.IsValid)
            {
                return View(request);
            }
            string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.USER_ID);
            Guid userId;

            Guid.TryParse(userIdString, out userId);
            // Chuyển đổi thành công
            request.StaffId = userId;
            if (request.ListOrderProduct != null && request.ListOrderProduct.Count > 0)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // If needed
                    WriteIndented = true // If needed
                };
                string listProductJson = System.Text.Json.JsonSerializer.Serialize(request.ListOrderProduct, options);
                request.ListOrderProductJson = listProductJson;
            }


            var status = await _OrderApiService.CreateOrderByStaff(request);

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

            TempData["SuccessMsg"] = "Tạo mới thành công cho " + request.PhoneNumber;

            return RedirectToAction("Index", "Order");
        }



        
    }



}

