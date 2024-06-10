using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Home
{
    public interface IHomeApiService
    {
        public Task<ApiResult<decimal>> TotalIncome();
        public Task<ApiResult<int>> TotalOrder();
        public Task<ApiResult<int>> AllOrderToday();
        public Task<ApiResult<List<decimal>>> IncomeAYear();
        public Task<ApiResult<List<OrderVm>>> RecentTransaction();
        public Task<ApiResult<List<OrderVm>>> RecentSuccessTransaction();
        public Task<ApiResult<List<OrderVm>>> RecentWaitTransaction();
        public Task<ApiResult<List<OrderVm>>> RecentFailTransaction();
        public Task<ApiResult<List<decimal>>> OrderByQuarter();
        public Task<ApiResult<int>> ViewNewCustomerOnDay();

    }
}
