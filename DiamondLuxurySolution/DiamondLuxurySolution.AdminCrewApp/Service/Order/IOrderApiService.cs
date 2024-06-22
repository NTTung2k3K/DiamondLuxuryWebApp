using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Order
{
    public interface IOrderApiService
    {
        public Task<ApiResult<bool>> CreateOrder(CreateOrderRequest request);
        public Task<ApiResult<bool>> DeleteOrder(string OrderId);
        public Task<ApiResult<bool>> UpdateShipper(UpdateShipperRequest request);
        public Task<ApiResult<bool>> CreateOrderByStaff(CreateOrderByStaffRequest request);

        public Task<ApiResult<bool>> UpdateInfoOrder(UpdateOrderRequest request);
        public Task<ApiResult<bool>> ContinuePayment(ContinuePaymentRequest request);

        public Task<ApiResult<bool>> ChangeStatusOrder(ChangeOrderStatusRequest request);
        public Task<ApiResult<OrderVm>> GetOrderById(string OrderId);
        public Task<ApiResult<PageResult<OrderVm>>> ViewOrder(ViewOrderRequest request);


    }
}
