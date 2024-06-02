using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Staff
{
    public class StaffApiService : BaseApiService, IStaffApiService
    {
        public StaffApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> ChangePasswordStaff(ChangePasswordStaffRequest request)
        {
            var data = await PostAsync<bool>("api/Staffs/ChangePasswordStaff", request);
            return data;
        }

        public  async Task<ApiResult<bool>> ChangeStatusCustomer(ChangeStatusCustomerRequest request)
        {
            var data = await PutAsync<bool>("api/Staffs/ChangeStatusCustomer", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteStaff(Guid StaffId)
        {
            var data = await DeleteAsync<bool>("api/Staffs/DeleteStaff/"+StaffId);
            return data;
        }

        public async Task<ApiResult<string>> ForgotpasswordStaffSendCode(string Username)
        {
            var data = await PostAsync<string>("api/ForgotStaffPassword/SendCode/",Username);
            return data;
        }

        public async Task<ApiResult<bool>> ForgotpassworStaffdChange(ForgotPasswordStaffChangeRequest request)
        {
            var data = await PostAsync<bool>("api/Staffs/ForgotCustomerPassword/ChangePassword", request);
            return data;
        }

        public async Task<ApiResult<StaffVm>> GetStaffById(Guid StaffId)
        {
            var data = await GetAsync<StaffVm>("api/Staffs/GetStaffById?StaffId="+StaffId);
            return data;
        }

        public async Task<ApiResult<StaffVm>> GetStaffByUsername(string Username)
        {
            var data = await GetAsync<StaffVm>("api/Staffs/GetStaffByUsername?Username=" + Username);
            return data;
        }

        public async Task<ApiResult<bool>> LoginStaff(LoginStaffRequest request)
        {
            var data = await PostAsync<bool>("api/Staffs/LoginStaff",request);
            return data;
        }

        public async Task<ApiResult<bool>> RegisterStaffAccount(CreateStaffAccountRequest request)
        {
            var data = await PostAsyncHasImage<bool>("api/Staffs/RegisterStaff", request);
            return data;
        }

        public async Task<ApiResult<bool>> UpdateStaffAccount(UpdateStaffAccountRequest request)
        {
            var data = await PostAsyncHasImage<bool>("api/Staffs/Update", request);
            return data;
        }

        public async Task<ApiResult<PageResult<StaffVm>>> ViewAdminPagination(ViewStaffPaginationCommonRequest request)
        {
            var data = await GetAsync<PageResult<StaffVm>>($"api/Staffs/ViewAdmin?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }

        public async Task<ApiResult<PageResult<CustomerVm>>> ViewCustomerPagination(ViewStaffPaginationCommonRequest request)
        {
            var data = await GetAsync<PageResult<CustomerVm>>($"api/Staffs/ViewCustomer?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }

        public async Task<ApiResult<PageResult<StaffVm>>> ViewDeliveryStaffPagination(ViewStaffPaginationCommonRequest request)
        {
            var data = await GetAsync<PageResult<StaffVm>>($"api/Staffs/ViewDeliveryStaff?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }

        public async Task<ApiResult<PageResult<StaffVm>>> ViewManagerPagination(ViewStaffPaginationCommonRequest request)
        {
            var data = await GetAsync<PageResult<StaffVm>>($"api/Staffs/ViewManagerStaff?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }

        public async Task<ApiResult<PageResult<StaffVm>>> ViewSalesStaffPagination(ViewStaffPaginationCommonRequest request)
        {
            var data = await GetAsync<PageResult<StaffVm>>($"api/Staffs/ViewSalesStaff?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
