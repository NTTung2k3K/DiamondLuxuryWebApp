using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;

namespace DiamondLuxurySolution.WebApp.Service.Order
{
    public interface IOrderApiService
    {
        public Task<ApiResult<OrderVm>> GetOrderById(string OrderId);
        public Task<ApiResult<PageResult<OrderVm>>> GetListOrderOfCustomer(ViewOrderRequest request);

        public Task<ApiResult<string>> CreateOrder(CreateOrderRequest request);

        public Task<ApiResult<string>> ChangeStatusOrder(ChangeOrderStatusRequest request);
        public Task<ApiResult<string>> ChangeStatusOrderPaypal(ChangeOrderStatusRequest request);


    }
}
