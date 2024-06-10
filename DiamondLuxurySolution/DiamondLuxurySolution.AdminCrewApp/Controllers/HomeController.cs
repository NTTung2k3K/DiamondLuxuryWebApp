using DiamondLuxurySolution.AdminCrewApp.Service.Home;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeApiService _homeApiService;

        public HomeController(IHomeApiService homeApiService)
        {
            _homeApiService = homeApiService;
        }

        public async Task<IActionResult> Index()
        {
            var totalIncome = await _homeApiService.TotalIncome();
            ViewBag.TotalIncome = totalIncome.ResultObj;

            var totalOrder = await _homeApiService.TotalOrder();
            ViewBag.TotalOrder = totalOrder.ResultObj;

            var orderToday = await _homeApiService.AllOrderToday();
            ViewBag.OrderToday = orderToday.ResultObj;

            var customerToday = await _homeApiService.ViewNewCustomerOnDay();
            ViewBag.CustomerToday = customerToday.ResultObj;

            var incomeAYear = await _homeApiService.IncomeAYear();
            ViewBag.IncomeAYear = incomeAYear.ResultObj;

            var recentTransaction = await _homeApiService.RecentTransaction();
            ViewBag.RecentTransaction = recentTransaction.ResultObj;

            var recentSuccessTransaction = await _homeApiService.RecentSuccessTransaction();
            ViewBag.RecentSuccessTransaction = recentSuccessTransaction.ResultObj;

            var recentFailTransaction = await _homeApiService.RecentFailTransaction();
            ViewBag.RecentFailTransaction = recentFailTransaction.ResultObj;

            var recentWaitTransaction = await _homeApiService.RecentWaitTransaction();
            ViewBag.RecentWaitTransaction = recentWaitTransaction.ResultObj;

            var orderByQuarter = await _homeApiService.OrderByQuarter();
            ViewBag.OrderByQuarter = orderByQuarter.ResultObj;

            
            
            return View();
        }
    }
}
