using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;

namespace DiamondLuxurySolution.WebApp.Service.Customer
{
    public interface ICustomerApiService
    {
        public Task<ApiResult<CustomerVm>> GetCustomerById(Guid CustomerId);
        public Task<ApiResult<bool>> DeleteCustomer(Guid CustomerId);

    }
}
