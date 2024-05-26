using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Order
{
    public class OrderRepo : IOrderRepo
    {
        public Task<ApiResult<bool>> AcceptOrder(ChangeOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> CancelOrder(ChangeOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> CreateOrder(CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }

        /* public Task<ApiResult<bool>> CreateOrder(CreateOrderRequest request)
         {
             List<string> errorList = new List<string>();
             if (string.IsNullOrEmpty(request.ShipName))
             {
                 errorList.Add("Vui lòng nhập tên người nhận hàng");
             }
             if (string.IsNullOrEmpty(request.ShipEmail))
             {
                 errorList.Add("Vui lòng nhập email nhận hàng");
             }
             if (string.IsNullOrEmpty(request.ShipPhoneNumber))
             {
                 errorList.Add("Vui lòng nhập số điện thoại người nhận hàng");
             }
             if (string.IsNullOrEmpty(request.ShipAdress))
             {
                 errorList.Add("Vui lòng nhập địa chỉ nhận hàng");
             }
             if()

         }*/

        public Task<ApiResult<bool>> DeleteOrder(ChangeOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<OrderVm>> GetOrderById(int OrderId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateOrder(UpdateOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PageResult<OrderVm>>> ViewOrder(ViewOrderRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
