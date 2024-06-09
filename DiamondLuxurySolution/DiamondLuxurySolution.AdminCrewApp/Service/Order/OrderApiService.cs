using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Order
{
    public class OrderApiService : BaseApiService, IOrderApiService
    {
        public OrderApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> ChangeStatusOrder(ChangeOrderStatusRequest request)
        {

            var data = await PutAsync<bool>("api/Orders/ChangeStatus", request);
            return data;
        }

        public async Task<ApiResult<bool>> ContinuePayment(ContinuePaymentRequest request)
        {
            var data = await PutAsync<bool>("api/Orders/PaidTheRest", request);
            return data;
        }

        public async Task<ApiResult<bool>> CreateOrder(CreateOrderRequest request)
        {
            var data = await PostAsync<bool>("api/Orders/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> CreateOrderByStaff(CreateOrderByStaffRequest request)
        {
            var data = await PostAsync<bool>("api/Orders/CreateOrderByStaff", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteOrder(string OrderId)
        {
            var data = await DeleteAsync<bool>($"api/Orders/Delete?OrderId={OrderId}");
            return data;
        }

        public async Task<ApiResult<OrderVm>> GetOrderById(string OrderId)
        {
            var data = await GetAsync<OrderVm>($"api/Orders/GetById?OrderId={OrderId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateInfoOrder(UpdateOrderRequest request)
        {
            var data = await PutAsync<bool>("api/Orders/Update",request);
            return data;
        }

        public async Task<ApiResult<bool>> UpdateShipper(UpdateShipperRequest request)
        {
            var data = await PutAsync<bool>("api/Orders/UpdateShipper", request);
            return data;
        }

        public async Task<ApiResult<PageResult<OrderVm>>> ViewOrder(ViewOrderRequest request)
        {
            var data = await GetAsync<PageResult<OrderVm>>($"api/Orders/View?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
