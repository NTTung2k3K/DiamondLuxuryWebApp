using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Product;
using System.Collections.Generic;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Home
{
    public class HomeApiService : BaseApiService, IHomeApiService
    {
        public HomeApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<int>> AllOrderToday()
        {
            var data = await GetAsync<int>("api/Orders/GetAllOrderToday");
            return data;
        }

        public async Task<ApiResult<List<decimal>>> IncomeAYear()
        {
            var data = await GetAsync<List<decimal>> ("api/Orders/GetIncomeAYear");
            return data;
        }

        public async Task<ApiResult<List<int>>> OrderByQuarter()
        {
            var data = await GetAsync< List<int>>("api/Orders/GetOrderByQuarter");
            return data;
        }

        public async Task<ApiResult<List<OrderVm>>> RecentFailTransaction()
        {
            var data = await GetAsync<List<OrderVm>>("api/Orders/GetRecentFailTransaction");
            return data;
        }

        public async Task<ApiResult<List<OrderVm>>> RecentSuccessTransaction()
        {
            var data = await GetAsync<List<OrderVm>>("api/Orders/GetRecentSuccessTransaction");
            return data;
        }

        public async Task<ApiResult<List<OrderVm>>> RecentTransaction()
        {
            var data = await GetAsync<List<OrderVm>>("api/Orders/GetRecentTransaction");
            return data;
        }

        public async Task<ApiResult<List<OrderVm>>> RecentWaitTransaction()
        {
            var data = await GetAsync<List<OrderVm>>("api/Orders/GetRecentWaitTransaction");
            return data;
        }

        public async Task<ApiResult<decimal>> TotalIncome()
        {
            var data = await GetAsync<decimal>("api/Orders/GetTotalIncome");
            return data;
        }

        public async Task<ApiResult<int>> TotalOrder()
        {
            var data = await GetAsync<int>("api/Orders/GetTotalOrder");
            return data;
        }
        public async Task<ApiResult<List<decimal>>> IncomeByWeek()
        {
            var data = await GetAsync<List<decimal>>($"api/Orders/GetIncomeByWeek");
            return data;
        }
        public async Task<ApiResult<int>> ViewNewCustomerOnDay()
        {
            var data = await GetAsync<int>("api/Staffs/GetNumberCustomerToday");
            return data;
        }
        public async Task<ApiResult<decimal>> IncomeToday()
        {
            var data = await GetAsync<decimal>($"api/Orders/GetIncomeToday");
            return data;
        }

        public async Task<ApiResult<int>> CountContactNotSolve()
        {
            var data = await GetAsync<int>($"api/Contact/CountContactNotSolve");
            return data;
        }

        public async Task<ApiResult<int>> CountAllNews()
        {
            var data = await GetAsync<int>($"api/News/CountAllNew");
            return data;
        }

        public async Task<ApiResult<List<int>>> CountAllCustomerInYear()
        {
            var data = await GetAsync<List<int>> ($"api/Customers/CountAllCustomerInYear");
            return data;
        }

        public async Task<ApiResult<int>> CountAllCustomer()
        {
            var data = await GetAsync<int>($"api/Staffs/CountAllCustomer");
            return data;
        }

        public async Task<ApiResult<List<ProductCategorySale>>> ViewProductCategorySale()
        {
            var data = await GetAsync<List<ProductCategorySale>>($"api/Products/ViewProductCategorySale");
            return data;
        }

        public async Task<ApiResult<List<ProductSaleChart>>> ViewProductSale12Days()
        {
            var data = await GetAsync<List<ProductSaleChart>>($"api/Products/ViewProductSale12Days");
            return data;
        }

        public async Task<ApiResult<List<int>>> ContactAWeek()
        {
            var data = await GetAsync<List<int>>($"api/Contact/ContactAWeek");
            return data;
        }

        public async Task<ApiResult<List<int>>> CountAllCustomerInWeek()
        {
            var data = await GetAsync<List<int>>($"api/Customers/CountAllCustomerInWeek");
            return data;
        }
    }
}
