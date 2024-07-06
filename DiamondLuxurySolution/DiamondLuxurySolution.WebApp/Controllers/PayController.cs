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

        public PayController(ICustomerApiService customerApiService, IPaymentApiService paymentApiService, IPromotionApiService promotionApiService, IOrderApiService orderApiService)
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
                HttpContext.Session.SetString("ReturnToPayInfor", "ReturnToPayInfor");
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

            // Adding Item Details like name, currency, price etc  
            var order = await _orderApiService.GetOrderById(OrderId);
            decimal subtotal = 0;
            foreach (var item in order.ResultObj.ListOrderProduct)
            {
                var itemPrice = Math.Round(item.UnitPrice / 25000, 2);
                subtotal += itemPrice * item.Quantity;

                itemList.items.Add(new Item()
                {
                    name = item.ProductName,
                    currency = "USD",
                    price = itemPrice.ToString("F2"),
                    quantity = item.Quantity.ToString(),
                    sku = item.ProductId
                });
            }

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

            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0.00",
                shipping = "0.00",
                subtotal = subtotal.ToString("F2")
            };

            // Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = subtotal.ToString("F2"), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };

            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Hóa đơn #{order.ResultObj.OrderId}",
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
                TempData["TotalAmount"] = subtotal.ToString("F2");
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
            var status = await _orderApiService.ChangeStatusOrder(request);

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
                ShipAdress = request.ShipAdress + ",Quận/Huyện " + billing_streetAddress + ",Tp. " + country,
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
    }
}
