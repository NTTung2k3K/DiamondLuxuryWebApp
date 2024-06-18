using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.WebApp.Services;

namespace DiamondLuxurySolution.WebApp.Service.Order
{
    public class OrderApiService : BaseApiService, IOrderApiService
    {
        public OrderApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<string>> CreateOrder(CreateOrderRequest request)
        {
            var data = await PostAsync<string>("api/Orders/Create", request);
            return data;
        }
        public async Task<ApiResult<PageResult<OrderVm>>> GetListOrderOfCustomer(ViewOrderRequest request)
        {
            var data = await GetAsync<PageResult<OrderVm>>($"api/Orders/GetFullOrderByCustomerId?CustomerId={request.CustomerId}&pageIndex={request.pageIndex}");
            return data;
        }

        public async Task<ApiResult<OrderVm>> GetOrderById(string OrderId)
        {
            var data = await GetAsync<OrderVm>($"api/Orders/GetById?OrderId={OrderId}");
            return data;
        }

        public async Task<ApiResult<string>> ChangeStatusOrder(ChangeOrderStatusRequest request)
        {

            var data = await PutAsync<string>("api/Orders/ChangeStatus", request);
            return data;
        }

    }
}
