using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult PageNotFound()
        {
            return View();
        }
        public IActionResult InternalServerError()
        {
            return View();
        }
        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}
