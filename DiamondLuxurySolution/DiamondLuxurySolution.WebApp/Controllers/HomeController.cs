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
            return View();
        }

       


    }
}
