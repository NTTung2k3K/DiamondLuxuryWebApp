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
        public Task<ApiResult<bool>> LoginStaff(LoginStaffRequest request);
        public Task<ApiResult<bool>> RegisterStaffAccount(CreateStaffAccountRequest request);
        public Task<ApiResult<PageResult<StaffVm>>> ViewStaffPagination(ViewStaffPaginationRequest request);

        public Task<ApiResult<bool>> UpdateStaffAccount(UpdateStaffAccountRequest request);
        public Task<ApiResult<StaffVm>> GetStaffById(Guid StaffId);

        public Task<ApiResult<bool>> DeleteStaff(Guid StaffId);
        public Task<ApiResult<bool>> ChangePasswordStaff(ChangePasswordStaffRequest request);
    }
}
