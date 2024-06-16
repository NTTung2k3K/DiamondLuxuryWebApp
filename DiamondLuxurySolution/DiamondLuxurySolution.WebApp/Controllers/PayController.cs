using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class PayController : Controller
    {
        public IActionResult Info()
        {
            var customerId = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID);
            if(customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}
