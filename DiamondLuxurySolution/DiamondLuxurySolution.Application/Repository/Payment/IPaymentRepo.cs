using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Payment;
using DiamondLuxurySolution.ViewModel.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Payment
{
    public interface IPaymentRepo
    {
        public Task<ApiResult<List<PaymentVm>>> GetAll();
        public Task<ApiResult<bool>> CreatePayment(CreatePaymentRequest request);
        public Task<ApiResult<bool>> UpdatePayment(UpdatePaymentRequest request);
        public Task<ApiResult<bool>> DeletePayment(DeletePaymentRequest request);
        public Task<ApiResult<PaymentVm>> GetPaymentById(Guid PaymentId);
        public Task<ApiResult<PageResult<PaymentVm>>> ViewPayment(ViewPaymentRequest request);
    }
}
