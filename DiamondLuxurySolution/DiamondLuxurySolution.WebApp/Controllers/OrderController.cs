using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.WebApp.Service.Order;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderApiService _OrderApiService;

        public OrderController(IOrderApiService orderApiService) 
        {
            _OrderApiService = orderApiService;
        }
        public async Task<IActionResult> ViewDetail(string OrderId)
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

        public async Task<IActionResult> ViewAll(ViewOrderRequest request)
        {
            try
            {
                string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID);
                Guid userId;



                Guid.TryParse(userIdString, out userId);
                request.CustomerId = userId;

                var Order = await _OrderApiService.GetListOrderOfCustomer(request);
                ViewBag.PaymentList = Order.ResultObj;

                if (Order is ApiErrorResult<PageResult<OrderVm>> errorResult)
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


    }
}
