using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.Warehouse;
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
        public Task<ApiResult<bool>> UpdateOrder(UpdateOrderRequest request);
        public Task<ApiResult<bool>> DeleteOrder(ChangeOrderRequest request);
        public Task<ApiResult<bool>> CancelOrder(ChangeOrderRequest request);
        public Task<ApiResult<bool>> AcceptOrder(ChangeOrderRequest request);
        public Task<ApiResult<OrderVm>> GetOrderById(int OrderId);
        public Task<ApiResult<PageResult<OrderVm>>> ViewOrder(ViewOrderRequest request);
    }
}
