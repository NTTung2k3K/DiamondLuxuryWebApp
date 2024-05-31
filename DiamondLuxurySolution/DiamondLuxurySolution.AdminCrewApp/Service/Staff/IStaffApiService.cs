using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Staff
{
    public interface IStaffApiService 
    {
        public Task<ApiResult<bool>> LoginStaff(LoginStaffRequest request);
        public Task<ApiResult<bool>> RegisterStaffAccount(CreateStaffAccountRequest request);


        public Task<ApiResult<bool>> UpdateStaffAccount(UpdateStaffAccountRequest request);
        public Task<ApiResult<StaffVm>> GetStaffById(Guid StaffId);

        public Task<ApiResult<bool>> DeleteStaff(Guid StaffId);

        public Task<ApiResult<bool>> ChangePasswordStaff(ChangePasswordStaffRequest request);
        public Task<ApiResult<string>> ForgotpasswordStaffSendCode(string Username);
        public Task<ApiResult<bool>> ForgotpassworStaffdChange(ForgotPasswordStaffChangeRequest request);

        public Task<ApiResult<bool>> ChangeStatusCustomer(ChangeStatusCustomerRequest request);

        public Task<ApiResult<PageResult<CustomerVm>>> ViewCustomerPagination(ViewStaffPaginationCommonRequest request);
        public Task<ApiResult<PageResult<StaffVm>>> ViewSalesStaffPagination(ViewStaffPaginationCommonRequest request);
        public Task<ApiResult<PageResult<StaffVm>>> ViewDeliveryStaffPagination(ViewStaffPaginationCommonRequest request);
        public Task<ApiResult<PageResult<StaffVm>>> ViewAdminPagination(ViewStaffPaginationCommonRequest request);
        public Task<ApiResult<PageResult<StaffVm>>> ViewManagerPagination(ViewStaffPaginationCommonRequest request);
    }
}
