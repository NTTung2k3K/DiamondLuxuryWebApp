using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Product;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Home
{
    public interface IHomeApiService
    {
        public Task<ApiResult<decimal>> TotalIncome();
        public Task<ApiResult<int>> TotalOrder();
        public Task<ApiResult<int>> AllOrderToday();
        public Task<ApiResult<decimal>> IncomeToday();

        public Task<ApiResult<List<decimal>>> IncomeAYear();
        public Task<ApiResult<List<OrderVm>>> RecentTransaction();
        public Task<ApiResult<List<OrderVm>>> RecentSuccessTransaction();
        public Task<ApiResult<List<OrderVm>>> RecentWaitTransaction();
        public Task<ApiResult<List<OrderVm>>> RecentFailTransaction();
        public Task<ApiResult<List<int>>> OrderByQuarter();

        public Task<ApiResult<List<decimal>>> IncomeByWeek();
        public Task<ApiResult<List<ProductCategorySale>>> ViewProductCategorySale();

        public Task<ApiResult<List<ProductSaleChart>>> ViewProductSale12Days();

        public Task<ApiResult<List<int>>> ContactAWeek();
        public Task<ApiResult<List<int>>> CountAllCustomerInWeek();


        //Admin

        public Task<ApiResult<int>> ViewNewCustomerOnDay();

        public Task<ApiResult<int>> CountContactNotSolve();
        public Task<ApiResult<int>> CountAllNews();
        public Task<ApiResult<List<int>>> CountAllCustomerInYear();

        public Task<ApiResult<int>> CountAllCustomer();

    }
}
