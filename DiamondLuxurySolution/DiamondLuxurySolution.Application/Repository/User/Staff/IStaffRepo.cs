using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.User.Staff
{
    public interface IStaffRepo
    {
        public Task<ApiResult<string>> LoginStaff(LoginStaffRequest request);
        public Task<ApiResult<bool>> RegisterStaffAccount(CreateStaffAccountRequest request);
      

        public Task<ApiResult<bool>> UpdateStaffAccount(UpdateStaffAccountRequest request);
        public Task<ApiResult<StaffVm>> GetStaffById(Guid StaffId);
        public Task<ApiResult<StaffVm>> GetStaffByUsername(string Username);

        public Task<ApiResult<bool>> DeleteStaff(Guid StaffId);
        
        public Task<ApiResult<bool>> ChangePasswordStaff(ChangePasswordStaffRequest request);
        public Task<ApiResult<string>> ForgotpasswordStaffSendCode(string Username);
        public Task<ApiResult<bool>> ForgotpassworStaffdChange(ForgotPasswordStaffChangeRequest request);

        public Task<ApiResult<bool>> ChangeStatusCustomer(ChangeStatusCustomerRequest request);
        public Task<ApiResult<int>> ViewNewCustomerOnDay();


        public Task<ApiResult<PageResult<CustomerVm>>> ViewCustomerPagination(ViewStaffPaginationCommonRequest request);
        public Task<ApiResult<PageResult<StaffVm>>> ViewSalesStaffPagination(ViewStaffPaginationCommonRequest request);
        public Task<ApiResult<PageResult<StaffVm>>> ViewDeliveryStaffPagination(ViewStaffPaginationCommonRequest request);
        public Task<ApiResult<PageResult<StaffVm>>> ViewAdminPagination(ViewStaffPaginationCommonRequest request);
        public Task<ApiResult<PageResult<StaffVm>>> ViewManagerPagination(ViewStaffPaginationCommonRequest request);



    }
}
