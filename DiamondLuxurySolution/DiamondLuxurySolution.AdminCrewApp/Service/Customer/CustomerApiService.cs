using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Customer
{
    public class CustomerApiService : BaseApiService, ICustomerApiService
    {
        public CustomerApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<bool>> DeleteCustomer(Guid CustomerId)
        {
            var data = await DeleteAsync<bool>("api/Customers/DeleteCustomer?CustomerId=" + CustomerId);
            return data;
        }

        public async Task<ApiResult<CustomerVm>> GetCustomerById(Guid CustomerId)
        {
            var data = await GetAsync<CustomerVm>("api/Customers/GetCustomerById?CustomerId=" + CustomerId);
            return data;
        } 
    }
}
