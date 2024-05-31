using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Payment;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Payment
{
    public interface IPaymentApiService
    {
        public Task<ApiResult<List<PaymentVm>>> GetAll();
        public Task<ApiResult<bool>> CreatePayment(CreatePaymentRequest request);
        public Task<ApiResult<bool>> UpdatePayment(UpdatePaymentRequest request);
        public Task<ApiResult<bool>> DeletePayment(DeletePaymentRequest request);
        public Task<ApiResult<PaymentVm>> GetPaymentById(Guid PaymentId);
        public Task<ApiResult<PageResult<PaymentVm>>> ViewInPayment(ViewPaymentRequest request);

    }
}
