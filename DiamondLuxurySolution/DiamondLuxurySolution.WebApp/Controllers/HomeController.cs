using Azure.Core;
using DiamondLuxurySolution.WebApp.Service.Slide;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class HomeController : Controller
    {
       
        // Trang chinh
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_NAME) == null)
            {
                var customerName = Request.Cookies["CustomerName"];
                var customerId = Request.Cookies["CustomerId"];

                if (!string.IsNullOrEmpty(customerName) && !string.IsNullOrEmpty(customerId))
                {
                    HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_NAME, customerName);
                    HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID, customerId);
                }
            }
            return View();
        }

       


    }
}
