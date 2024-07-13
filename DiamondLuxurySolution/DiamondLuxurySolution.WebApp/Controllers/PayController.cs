using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.WebApp.Models;
using DiamondLuxurySolution.WebApp.Service.Customer;
using DiamondLuxurySolution.WebApp.Service.Order;
using DiamondLuxurySolution.WebApp.Service.Payment;
using DiamondLuxurySolution.WebApp.Service.Promotion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PayPal;
using PayPal.Api;
using System.Text.Json;
using Payment = PayPal.Api.Payment;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class PayController : Controller
    {
        private readonly ICustomerApiService _customerApiService;
        private readonly IPaymentApiService _paymentApiService;
        private readonly IPromotionApiService _promotionApiService;
        private readonly IOrderApiService _orderApiService;
        private readonly VnPaySettings _vnPaySettings;

        public PayController(IOptions<VnPaySettings> vnPaySettings, ICustomerApiService customerApiService, IPaymentApiService paymentApiService, IPromotionApiService promotionApiService, IOrderApiService orderApiService)
        {
            _customerApiService = customerApiService;
            _paymentApiService = paymentApiService;
            _promotionApiService = promotionApiService;
            _orderApiService = orderApiService;
            _vnPaySettings = vnPaySettings.Value;

        }


        [HttpGet]
        public async Task<IActionResult> Info()
        {
            var customerId = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID);
            if (customerId == null)
            {
                HttpContext.Session.SetString("ReturnToPayInfor", "ReturnToPayInfor");
                return RedirectToAction("Login", "Account");
            }

            Guid userId;

            Guid.TryParse(customerId, out userId);
            // Chuyển đổi thành công

            var listPayment = await _paymentApiService.GetAll();
            ViewBag.OnlinePaymentId = listPayment.ResultObj.Find(x => x.PaymentMethod == "Thanh Toán Trực Tuyến").PaymentId;
            ViewBag.CodID = listPayment.ResultObj.Find(x => x.PaymentMethod == "COD").PaymentId;

            var listPromotion = await _promotionApiService.GetAllOnTime();
            ViewBag.ListPromotionOnTime = listPromotion.ResultObj;

            if (listPromotion.ResultObj != null && listPromotion.ResultObj.Count > 0)
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


        #region paypal payment
        //Paypal start

        public async Task<ActionResult> PaymentWithPaypalAndDeposit(string OrderId, string Cancel = null)
        {
            // Getting the APIContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                // A resource representing a Payer that funds a payment Payment Method as paypal  
                // Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Query["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    // This section will be executed first because PayerID doesn't exist  
                    // it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sends back the data.  
                    string baseURI = $"{Request.Scheme}://{Request.Host}/Pay/PaymentWithPayPal?";
                    // Here we are generating guid for storing the paymentID received in session  
                    // which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    // Store OrderId in session
                    HttpContext.Session.SetString("OrderId", OrderId);
                    // CreatePayment function gives us the payment approval url  
                    // on which payer is redirected for paypal account payment  
                    Payment createdPayment = await this.CreatePaymentWithDeposit(apiContext, baseURI + "guid=" + guid, OrderId);
                    // Get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            // Saving the paypal redirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // Saving the paymentID in the key guid  
                    HttpContext.Session.SetString(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function executes after receiving all parameters for the payment  
                    var guid = Request.Query["guid"];
                    // Retrieve OrderId from session
                    var orderId = HttpContext.Session.GetString("OrderId");
                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Session.GetString(guid) as string);
                    // If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (PaymentsException ex)
            {
                // Log detailed error message from PayPal
                Console.WriteLine($"PayPal PaymentsException: {ex.Response}");
                return View("FailureView");
            }
            catch (Exception ex)
            {
                // Log general exception
                Console.WriteLine($"Exception: {ex.Message}");
                return View("FailureView");
            }
            // On successful payment, show success page to user.  
            TempData["PaymentStatus"] = DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString();
            TempData["OrderId"] = OrderId;
            TempData["TotalAmountSuccess"] = TempData["TotalAmount"];

            return RedirectToAction("PaySuccess", "Pay");
        }



        public async Task<ActionResult> PaymentWithPaypal(string OrderId, string Cancel = null)
        {
            // Getting the APIContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                // A resource representing a Payer that funds a payment Payment Method as paypal  
                // Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Query["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    // This section will be executed first because PayerID doesn't exist  
                    // it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sends back the data.  
                    string baseURI = $"{Request.Scheme}://{Request.Host}/Pay/PaymentWithPayPal?";
                    // Here we are generating guid for storing the paymentID received in session  
                    // which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    // Store OrderId in session
                    HttpContext.Session.SetString("OrderId", OrderId);
                    // CreatePayment function gives us the payment approval url  
                    // on which payer is redirected for paypal account payment  
                    Payment createdPayment = await this.CreatePayment(apiContext, baseURI + "guid=" + guid, OrderId);
                    // Get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            // Saving the paypal redirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // Saving the paymentID in the key guid  
                    HttpContext.Session.SetString(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function executes after receiving all parameters for the payment  
                    var guid = Request.Query["guid"];
                    // Retrieve OrderId from session
                    var orderId = HttpContext.Session.GetString("OrderId");
                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Session.GetString(guid) as string);
                    // If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (PaymentsException ex)
            {
                // Log detailed error message from PayPal
                Console.WriteLine($"PayPal PaymentsException: {ex.Response}");
                return View("FailureView");
            }
            catch (Exception ex)
            {
                // Log general exception
                Console.WriteLine($"Exception: {ex.Message}");
                return View("FailureView");
            }
            // On successful payment, show success page to user.  
            TempData["PaymentStatus"] = DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString();
            TempData["OrderId"] = OrderId;
            TempData["TotalAmountSuccess"] = TempData["TotalAmount"];

            return RedirectToAction("PaySuccess", "Pay");
        }

        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }


        private async Task<Payment> CreatePaymentWithDeposit(APIContext apiContext, string redirectUrl, string OrderId)
        {
            // Create item list and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            // Retrieve the order details
            var order = await _orderApiService.GetOrderById(OrderId);
            // Calculate the total item prices
            decimal totalItemPrice = 0;
            foreach (var item in order.ResultObj.ListOrderProduct)
            {
                var itemPrice = Math.Round(item.UnitPrice / 25000, 2);
                totalItemPrice += itemPrice * item.Quantity;

                itemList.items.Add(new Item()
                {
                    name = item.ProductName,
                    currency = "USD",
                    price = itemPrice.ToString("F2"),
                    quantity = item.Quantity.ToString(),
                    sku = item.ProductId
                });
            }

            // Calculate the deposit amount for the order (or any other custom amount)
            var listOrderPayment = order.ResultObj.OrdersPaymentVm
                .OrderByDescending(x => x.OpenPaymentTime).FirstOrDefault();
            decimal depositAmount = Math.Round((decimal)listOrderPayment.PaymentAmount / 25000, 2);

            // Update the itemList to reflect the deposit amount instead of full price
            itemList.items.Clear(); // Clear existing items
            itemList.items.Add(new Item()
            {
                name = "Deposit for Order " + OrderId,
                currency = "USD",
                price = depositAmount.ToString("F2"),
                quantity = "1",
                sku = "DEPOSIT"
            });

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = $"{redirectUrl}&Cancel=true&OrderId={OrderId}",
                return_url = $"{redirectUrl}&OrderId={OrderId}"
            };

            // Adding Tax, shipping, and Subtotal details  
            var details = new Details()
            {
                tax = "0.00",
                shipping = "0.00",
                subtotal = depositAmount.ToString("F2")
            };

            // Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = depositAmount.ToString("F2"), // Total must be equal to the sum of tax, shipping, and subtotal.  
                details = details
            };

            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Deposit for Order #{order.ResultObj.OrderId}",
                invoice_number = paypalOrderId.ToString(), // Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using an APIContext  
            try
            {
                TempData["TotalAmount"] = depositAmount.ToString("F2");
                var createdPayment = this.payment.Create(apiContext);

                // Log the request and response
                Console.WriteLine("Payment request: " + this.payment.ConvertToJson());
                Console.WriteLine("Payment response: " + createdPayment.ConvertToJson());

                return createdPayment;
            }
            catch (PaymentsException ex)
            {
                // Log detailed error message from PayPal
                Console.WriteLine($"PayPal PaymentsException: {ex.Response}");
                throw;
            }
            catch (Exception ex)
            {
                // Log general exception
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }


        private async Task<Payment> CreatePayment(APIContext apiContext, string redirectUrl, string OrderId)
        {
            // Create item list and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            // Retrieve the order details
            var order = await _orderApiService.GetOrderById(OrderId);
            if (order == null || order.ResultObj == null)
            {
                throw new Exception("Order not found");
            }

            // Calculate the order total amount in USD
            decimal orderTotalUSD = Math.Round(order.ResultObj.TotalAmount / 25000, 2);

            // Add a single item reflecting the order total amount
            itemList.items.Add(new Item()
            {
                name = "Order Total for Order " + OrderId,
                currency = "USD",
                price = orderTotalUSD.ToString("F2"),
                quantity = "1",
                sku = "ORDER_TOTAL"
            });

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = $"{redirectUrl}&Cancel=true&OrderId={OrderId}",
                return_url = $"{redirectUrl}&OrderId={OrderId}"
            };

            // Adding Tax, shipping, and Subtotal details  
            var details = new Details()
            {
                tax = "0.00",
                shipping = "0.00",
                subtotal = orderTotalUSD.ToString("F2")
            };

            // Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = orderTotalUSD.ToString("F2"), // Total must be equal to the sum of tax, shipping, and subtotal.  
                details = details
            };

            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Order Total for Order #{order.ResultObj.OrderId}",
                invoice_number = paypalOrderId.ToString(), // Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using an APIContext  
            try
            {
                TempData["TotalAmount"] = orderTotalUSD.ToString("F2");
                var createdPayment = this.payment.Create(apiContext);

                // Log the request and response
                Console.WriteLine("Payment request: " + this.payment.ConvertToJson());
                Console.WriteLine("Payment response: " + createdPayment.ConvertToJson());

                return createdPayment;
            }
            catch (PaymentsException ex)
            {
                // Log detailed error message from PayPal
                Console.WriteLine($"PayPal PaymentsException: {ex.Response}");
                throw;
            }
            catch (Exception ex)
            {
                // Log general exception
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }






        [HttpGet]
        public async Task<IActionResult> PaySuccess(ChangeOrderStatusRequest request)
        {

            var statusTemp = TempData["PaymentStatus"];
            var orderId = TempData["OrderId"];
            var paymentString = TempData["TotalAmountSuccess"] as string;

            if (paymentString != null && decimal.TryParse(paymentString, out decimal paymentAmount))
            {
                request.Status = statusTemp?.ToString();
                request.OrderId = orderId?.ToString();
                request.PaymentAmount = paymentAmount;
            }
            var status = await _orderApiService.ChangeStatusOrderPaypal(request);

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
                return RedirectToAction("ViewDetail", "Order", new { OrderId = status.ResultObj });
            }
            return RedirectToAction("ViewDetail", "Order", new { OrderId = status.ResultObj });


        }


        //Paypal End
        #endregion




        [HttpPost]
        public async Task<IActionResult> Info(CreateOrderRequest request, string country)
        {
            var listPayment = await _paymentApiService.GetAll();
            ViewBag.PaypalId = listPayment.ResultObj.Find(x => x.PaymentMethod == "Paypal").PaymentId;
            ViewBag.CodID = listPayment.ResultObj.Find(x => x.PaymentMethod == "COD").PaymentId;
            var customerId = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID);
            Guid userId;
            Guid.TryParse(customerId, out userId);



            var listPromotion = await _promotionApiService.GetAll();
            ViewBag.ListPromotionOnTime = listPromotion.ResultObj;

            if (listPromotion.ResultObj != null && listPromotion.ResultObj.Count > 0)
            {
                ViewBag.FistPromotion = listPromotion.ResultObj.First().DiscountPercent;
            }
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
                ShipAdress = request.ShipAdress + ",Tp. " + country,
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
            return RedirectToAction("ViewDetail", "Order", new { OrderId = status.ResultObj });

        }











        #region Thanh toán vnpay

        [HttpPost]

        public async Task<IActionResult> PaymentVnPay(string orderId, int TypePaymentVN)
        {

            var url = await UrlPaymentAsync(TypePaymentVN, orderId);

            return Redirect(url);

        }
        [HttpPost]

        public async Task<IActionResult> PaymentVnPayWithDeposit(string orderId, int TypePaymentVN)
        {

            var url = await UrlPaymentAsyncWithDeposit(TypePaymentVN, orderId);

            return Redirect(url);

        }

        public async Task<string> UrlPaymentAsyncWithDeposit(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = await _orderApiService.GetOrderById(orderCode);
            if (order == null || order.ResultObj == null)
            {
                throw new Exception("Order not found");
            }
            var listOrderPayment = order.ResultObj.OrdersPaymentVm
               .OrderByDescending(x => x.OpenPaymentTime).FirstOrDefault();
            decimal depositAmount = Math.Round((decimal)listOrderPayment.PaymentAmount, 2);

            // Calculate the order total amount in USD

            //Get Config Info

            string vnp_Returnurl = _vnPaySettings.ReturnUrl;
            string vnp_Url = _vnPaySettings.Url;
            string vnp_TmnCode = _vnPaySettings.TmnCode;
            string vnp_HashSecret = _vnPaySettings.HashSecret;

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)depositAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.AddHours(1).ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.ResultObj.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.ResultObj.OrderId + "_" + Guid.NewGuid().ToString()); // Tạo mã tham chiếu duy nhất

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }


        #region Vnpay 100%
        public async Task<string> UrlPaymentAsync(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = await _orderApiService.GetOrderById(orderCode);
            if (order == null || order.ResultObj == null)
            {
                throw new Exception("Order not found");
            }

            // Calculate the order total amount in USD

            //Get Config Info

            string vnp_Returnurl = _vnPaySettings.ReturnUrl;
            string vnp_Url = _vnPaySettings.Url;
            string vnp_TmnCode = _vnPaySettings.TmnCode;
            string vnp_HashSecret = _vnPaySettings.HashSecret;

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.ResultObj.TotalAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.AddHours(1).ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.ResultObj.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.ResultObj.OrderId); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }

        public async Task<IActionResult> VnpayReturn()
        {
            if (Request.Query.Count > 0)
            {
                string vnp_HashSecret = _vnPaySettings.HashSecret; // Retrieve from appsettings.json
                var vnpayData = Request.Query;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (var pair in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(pair.Key) && pair.Key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(pair.Key, pair.Value);
                    }
                }

                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                if (orderCode.Length > 11)
                {
                    // Tách chuỗi theo dấu _
                    var parts = orderCode.Split('_');
                    if (parts.Length > 1)
                    {
                        orderCode = parts[0];
                    }
                    else
                    {
                        throw new Exception("Invalid transaction reference format");
                    }
                }
               

                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.Query["vnp_SecureHash"];
                String TerminalID = Request.Query["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.Query["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {

                        var request = new ChangeOrderStatusRequest();
                        request.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString();
                        request.OrderId = orderCode.ToString();
                        request.PaymentAmount = vnp_Amount;
                        var status = await _orderApiService.ChangeStatusOrder(request);

                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        return RedirectToAction("ViewDetail", "Order", new { OrderId = status.ResultObj });
                    }
                    else
                    {
                        var request = new ChangeOrderStatusRequest();
                        request.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Failed.ToString();
                        request.OrderId = orderCode.ToString();

                        request.PaymentAmount = vnp_Amount;
                        var status = await _orderApiService.ChangeStatusOrder(request);

                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý. Mã lỗi: " + vnp_ResponseCode;
                        return RedirectToAction("ViewDetail", "Order", new { OrderId = status.ResultObj });

                    }
                }
            }
            return View();
        }

        #endregion
        #endregion




    }
}
