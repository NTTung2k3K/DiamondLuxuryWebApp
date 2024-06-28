using Azure.Core;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.WebApp.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DiamondLuxurySolution.WebApp.Service.Account
{
    public class AccountApiService : BaseApiService, IAccountApiService
    {
        public AccountApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> ChangePasswordCustomer(ChangePasswordCustomerRequest request)
        {
            var data = await PutAsync<bool>("api/Customers/ChangePasswordCustomer", request);
            return data;
        }

        public async Task<ApiResult<bool>> ForgotpasswordCustomerChange(ForgotPasswordCustomerChangeRequest request)
        {
            var data = await PostAsync<bool>("api/Customers/ForgotCustomerPassword/ChangePassword", request);
            return data;
        }

        public async Task<ApiResult<string>> ForgotpasswordCustomerSendCode(string Email)
        {
            var data = await PostAsync<string>("api/Customers/ForgotCustomerPassword/SendCode", Email);
            return data;
        }

        public async Task<ApiResult<CustomerVm>> GetCustomerByEmail(string Email)
        {
            var data = await GetAsync<CustomerVm>("api/Customers/GetCustomerByEmail?Email=" + Email);
            return data;
        }

        public async Task<ApiResult<CustomerVm>> GetCustomerById(Guid CustomerId)
        {
            var data = await GetAsync<CustomerVm>("api/Customers/GetCustomerById?CustomerId=" + CustomerId);
            return data;
        }

        public async Task<ApiResult<PageResult<OrderVm>>> GetListOrderOfCustomer(ViewOrderRequest request)
        {
            var data = await GetAsync<PageResult<OrderVm>>("api/Orders/GetFullOrderByCustomerId?CustomerId=" + request.CustomerId+"&pageIndex="+request.pageIndex);
            return data;
        }

        public async Task<ApiResult<bool>> Login(LoginCustomerRequest request)
        {
            var data = await PostAsync<bool>("api/Customers/LoginCustomer", request);
            return data;
        }

       

        public async Task<ApiResult<bool>> Register(RegisterCustomerAccountRequest request)
        {
            var data = await PostAsync<bool>("api/Customers/RegisterCustomer", request);
            return data;
        }
        public async Task<ApiResult<bool>> CheckRegister(RegisterCustomerAccountRequest request)
        {
            var data = await PostAsync<bool>("api/Customers/CheckRegister", request);
            return data;
        }
        public async Task<ApiResult<bool>> UpdateCustomerAccount(UpdateCustomerRequest request)
        {
            var data = await PutAsync<bool>("api/Customers/Update", request);
            return data;
        }
    }
}
