using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.WebApp.Models;
using DiamondLuxurySolution.WebApp.Service.Customer;
using DiamondLuxurySolution.WebApp.Service.Order;
using DiamondLuxurySolution.WebApp.Service.Payment;
using DiamondLuxurySolution.WebApp.Service.Promotion;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class PayController : Controller
    {
        private readonly ICustomerApiService _customerApiService;
        private readonly IPaymentApiService _paymentApiService;
        private readonly IPromotionApiService _promotionApiService;
        private readonly IOrderApiService _orderApiService;

        public PayController(ICustomerApiService customerApiService, IPaymentApiService paymentApiService,IPromotionApiService promotionApiService,IOrderApiService orderApiService)
        {
            _customerApiService = customerApiService;
            _paymentApiService = paymentApiService;
            _promotionApiService = promotionApiService;
            _orderApiService = orderApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Info()
        {
            var customerId = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID);
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            Guid userId;

            Guid.TryParse(customerId, out userId);
            // Chuyển đổi thành công

            var listPayment = await _paymentApiService.GetAll();
            ViewBag.PaypalId = listPayment.ResultObj.Find(x => x.PaymentMethod == "Paypal").PaymentId;
            ViewBag.CodID = listPayment.ResultObj.Find(x => x.PaymentMethod == "COD").PaymentId;

            var listPromotion = await _promotionApiService.GetAllOnTime();
            ViewBag.ListPromotionOnTime = listPromotion.ResultObj;

            if(listPromotion.ResultObj != null && listPromotion.ResultObj.Count > 0)
            {
                ViewBag.FistPromotion = listPromotion.ResultObj.First().DiscountPercent;
            }

            var customer = await _customerApiService.GetCustomerById(userId);
            var orderVm = new CreateOrderRequest()
            {
                ShipAdress = customer.ResultObj.Address,
                ShipEmail = customer.ResultObj.Email,
                ShipName = customer.ResultObj.FullName,
                ShipPhoneNumber = customer.ResultObj.PhoneNumber,
            };
            

            return View(orderVm);
        }

        [HttpPost]
        public async Task<IActionResult> Info(CreateOrderRequest request, string country, string billing_streetAddress)
        {
            var listPayment = await _paymentApiService.GetAll();
            ViewBag.PaypalId = listPayment.ResultObj.Find(x => x.PaymentMethod == "Paypal").PaymentId;
            ViewBag.CodID = listPayment.ResultObj.Find(x => x.PaymentMethod == "COD").PaymentId;
            var customerId = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID);
            Guid userId;
            Guid.TryParse(customerId, out userId);



            var listPromotion = await _promotionApiService.GetAll();
            ViewBag.ListPromotionOnTime = listPromotion.ResultObj;


            var listOrderProduct = CartSessionHelper.GetCart();
            var listOrderProductVm = new List<OrderProductSupport>();
            foreach (var item in listOrderProduct)
            {
                var product = new OrderProductSupport()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Size = item.Ni,
                };
                listOrderProductVm.Add(product);
            }
            
            var orderVm = new CreateOrderRequest()
            {
                ShipAdress = request.ShipAdress + ",Quận " + billing_streetAddress + ", " + country,
                ShipEmail = request.ShipEmail,
                ShipName = request.ShipName,
                ShipPhoneNumber = request.ShipPhoneNumber,
                CustomerId = userId,
                Deposit = request.Deposit,
                Description = request.Description,
                ListPaymentId = request.ListPaymentId,
                PromotionId = request.PromotionId,
            };
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // If needed
                WriteIndented = true // If neededf
            };
            string listSubGemsJson = System.Text.Json.JsonSerializer.Serialize(listOrderProductVm, options);
            orderVm.ListOrderProductJson = listSubGemsJson;

            var status = await _orderApiService.CreateOrder(orderVm);

            if (status is ApiErrorResult<string> errorResult)
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

            TempData["SuccessMsg"] = "Tạo mới thành công";

            CartSessionHelper.ClearCart();
            return RedirectToAction("ViewDetail", "Order", new { OrderId = status.ResultObj});

        }
    }
}
