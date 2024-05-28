using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Order
{
    public interface IOrderRepo
    {
        public Task<ApiResult<bool>> CreateOrder(CreateOrderRequest request);
        public Task<ApiResult<bool>> DeleteOrder(string OrderId);
        public Task<ApiResult<bool>> UpdateInfoOrder(UpdateOrderRequest request);
        public Task<ApiResult<bool>> ContinuePayment(ContinuePaymentRequest request);

        public Task<ApiResult<bool>> ChangeStatusOrder(ChangeOrderStatusRequest request);
        public Task<ApiResult<OrderVm>> GetOrderById(string OrderId);
        public Task<ApiResult<PageResult<OrderVm>>> ViewOrder(ViewOrderRequest request);
    }
}
