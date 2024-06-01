using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Login
{
    public interface ILoginApiService
    {
        public Task<ApiResult<string>> LoginStaff(LoginStaffRequest request);
        public Task<ApiResult<string>> ForgotpasswordStaffSendCode(string Username);
        public Task<ApiResult<bool>> ForgotpassworStaffdChange(ForgotPasswordStaffChangeRequest request);

    }
}
