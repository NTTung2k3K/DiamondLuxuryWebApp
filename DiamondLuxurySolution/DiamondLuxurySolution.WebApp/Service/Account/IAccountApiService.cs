using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;

namespace DiamondLuxurySolution.WebApp.Service.Account
{
    public interface IAccountApiService
    {
        public Task<ApiResult<CustomerVm>> GetCustomerByEmail(string Email);

        public Task<ApiResult<bool>> Login(LoginCustomerRequest request);
        public Task<ApiResult<bool>> Register(RegisterCustomerAccountRequest request);
        public Task<ApiResult<bool>> UpdateCustomerAccount(UpdateCustomerRequest request);
        public Task<ApiResult<CustomerVm>> GetCustomerById(Guid CustomerId);
        public Task<ApiResult<bool>> ChangePasswordCustomer(ChangePasswordCustomerRequest request);
        public Task<ApiResult<PageResult<OrderVm>>> GetListOrderOfCustomer(ViewOrderRequest request);

        public Task<ApiResult<string>> ForgotpasswordCustomerSendCode(string Email);
        public Task<ApiResult<bool>> ForgotpasswordCustomerChange(ForgotPasswordCustomerChangeRequest request);
    }
}
