using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class DiamondJewelryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
