using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Login
{
    public class LoginApiService: BaseApiService, ILoginApiService
    {
        public LoginApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<string>> ForgotpasswordStaffSendCode(string Username)
        {
            var data = await PostAsync<string>("api/Staffs/ForgotStaffPassword/SendCode/"+Username, Username);
            return data;
        }

        public async Task<ApiResult<bool>> ForgotpassworStaffdChange(ForgotPasswordStaffChangeRequest request)
        {
            var data = await PostAsync<bool>("api/Staffs/ForgotCustomerPassword/ChangePassword", request);
            return data;
        }

        public async Task<ApiResult<string>> LoginStaff(LoginStaffRequest request)
        {
            var data = await PostAsync<string>("api/Staffs/LoginStaff", request);
            return data;
        }

       
    }
}
