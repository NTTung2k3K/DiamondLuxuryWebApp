using DiamondLuxurySolution.AdminCrewApp.Service.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHomeApiService _homeApiService;

        public HomeController(IHomeApiService homeApiService)
        {
            _homeApiService = homeApiService;
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager)]
        public async Task<IActionResult> Manager()
        {
            var totalIncome = await _homeApiService.TotalIncome();
            ViewBag.TotalIncome = totalIncome.ResultObj;

            var totalOrder = await _homeApiService.TotalOrder();
            ViewBag.TotalOrder = totalOrder.ResultObj;

            var orderToday = await _homeApiService.AllOrderToday();
            ViewBag.OrderToday = orderToday.ResultObj;

            var incomeToday = await _homeApiService.IncomeToday();
            ViewBag.IncomeToday = incomeToday.ResultObj;

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

            var ListIncomeByWeek = await _homeApiService.IncomeByWeek();
            ViewBag.ListIncomeByWeek = ListIncomeByWeek.ResultObj;

            ViewBag.TotalIncomeByWeek = ListIncomeByWeek.ResultObj.Sum();


            var ViewProductCategorySale = await _homeApiService.ViewProductCategorySale();
            ViewBag.ListProductCategorySale = ViewProductCategorySale.ResultObj;

            var ViewProductSale12Days = await _homeApiService.ViewProductSale12Days();
            ViewBag.ViewProductSale12Days = ViewProductSale12Days.ResultObj;

           


            return View();
        }
        [Authorize(Roles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin)]
        public async Task<IActionResult> Admin()
        {
            var allCustomerToday = await _homeApiService.ViewNewCustomerOnDay();
            ViewBag.AllCustomerToday = allCustomerToday.ResultObj;

            var sountContactNotSolve = await _homeApiService.CountContactNotSolve();
            ViewBag.CountContactNotSolve = sountContactNotSolve.ResultObj;

            var countAllNews = await _homeApiService.CountAllNews();
            ViewBag.CountAllNews = countAllNews.ResultObj;

            var countAllCustomerInYear = await _homeApiService.CountAllCustomerInYear();
            ViewBag.CountAllCustomerInYear = countAllCustomerInYear.ResultObj;

            var countAllCustomer = await _homeApiService.CountAllCustomer();
            ViewBag.CountAllCustomer = countAllCustomer.ResultObj;

            var ContactAWeek = await _homeApiService.ContactAWeek();
            ViewBag.ContactAWeek = ContactAWeek.ResultObj;

            var CountAllCustomerInWeek = await _homeApiService.CountAllCustomerInWeek();
            ViewBag.CountAllCustomerInWeek = CountAllCustomerInWeek.ResultObj;

            return View();
        }


    }
}
