using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Order;
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

        public async Task<ApiResult<List<decimal>>> OrderByQuarter()
        {
            var data = await GetAsync< List<decimal>>("api/Orders/GetOrderByQuarter");
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

        public async Task<ApiResult<int>> ViewNewCustomerOnDay()
        {
            var data = await GetAsync<int>("api/Staffs/GetNumberCustomerToday");
            return data;
        }
    }
}
