using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PagedList;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;

namespace DiamondLuxurySolution.Application.Repository.User.Customer
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly LuxuryDiamondShopContext _context;

        public CustomerRepo(LuxuryDiamondShopContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<ApiResult<bool>> ChangePasswordCustomer(ChangePasswordCustomerRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.CustomerId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Khách hàng không tồn tại");
            }
            var comfirmPassword = await _userManager.CheckPasswordAsync(user, request.OldPassword);
            if (comfirmPassword == false)
            {
                return new ApiErrorResult<bool>("Sai mật khẩu hiện tại");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                var errorApi = new ApiErrorResult<bool>("Lỗi thông tin");
                errorApi.ValidationErrors = new List<string>();
                foreach (var item in changePasswordResult.Errors)
                {
                    errorApi.ValidationErrors.Add(item.Description);
                }
                return errorApi;
            }
            user.LastChangePasswordTime = DateTime.Now;

            var statusUser = await _userManager.UpdateAsync(user);
            if (!statusUser.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống, thay đổi mật khẩu thất bại vui lòng thử lại");
            }
            return new ApiSuccessResult<bool>("Cập nhật mật khẩu thành công");
        }

    

        public async Task<ApiResult<bool>> DeleteCustomer(Guid CustomerId)
        {
            var user = await _userManager.FindByIdAsync(CustomerId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Khách hàng không tồn tại");
            }
            user.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CustomerStatus.Suspended.ToString();
            var statusUser = await _userManager.UpdateAsync(user);
            if (!statusUser.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống,xóa khách hàng thất bại vui lòng thử lại");
            }
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<CustomerVm>> GetCustomerById(Guid CustomerId)
        {
            var user = await _userManager.FindByIdAsync(CustomerId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<CustomerVm>("Khách hàng không tồn tại");
            }
            var customerVm = new CustomerVm()
            {
                CustomerId = user.Id,
                Dob = (DateTime)user.Dob,
                FullName = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            var listRoleOfUser = await _userManager.GetRolesAsync(user);

            if (listRoleOfUser.Count > 0)
            {
                customerVm.ListRoleName = new List<string>();
                foreach (var role in listRoleOfUser)
                {
                    customerVm.ListRoleName.Add(role);
                }
            }
            return new ApiSuccessResult<CustomerVm>(customerVm, "Success");
        }

        public async Task<ApiResult<bool>> Login(LoginCustomerRequest request)
        {
            
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return new ApiErrorResult<bool>("Tải khoản hoặc mật khẩu không đúng");
            }
            if (!DiamondLuxurySolution.Utilities.Helper.CheckValidInput.IsValidEmail(request.Email))
            {
                return new ApiErrorResult<bool>("Email không hợp lệ");
            }
            var user = await _userManager.FindByEmailAsync(request.Email.Trim());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tải khoản hoặc mật khẩu không đúng");
            }
            var userPasswordConfirm = await _userManager.CheckPasswordAsync(user, request.Password.Trim());
            if (userPasswordConfirm == false)
            {
                return new ApiErrorResult<bool>("Tải khoản hoặc mật khẩu không đúng");
            }
            
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> Register(RegisterCustomerAccountRequest request)
        {
            if (!request.Password.Equals(request.ConfirmPassword))
            {
                return new ApiErrorResult<bool>("Password không trùng khớp");
            }
            var userFindByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userFindByEmail != null)
            {
                return new ApiErrorResult<bool>("Email đã tồn tại");
            }
            if (DiamondLuxurySolution.Utilities.Helper.CheckValidInput.ContainsLetters(request.PhoneNumber))
            {
                return new ApiErrorResult<bool>("Số điện thoại không hợp lệ");
            }
            if (!DiamondLuxurySolution.Utilities.Helper.CheckValidInput.ValidLenghPhoneNumber(request.PhoneNumber))
            {
                return new ApiErrorResult<bool>("Số điện thoại không hợp lệ");
            }
            if (!DiamondLuxurySolution.Utilities.Helper.CheckValidInput.IsValidEmail(request.Email))
            {
                return new ApiErrorResult<bool>("Email không hợp lệ");
            }
            Random rd = new Random();
            string username = "CUS" + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString();

            var user = new AppUser()
            {
                Fullname = request.FullName.Trim(),
                Email = request.Email.Trim(),
                Dob = request.Dob,
                PhoneNumber = request.PhoneNumber.Trim(),
                UserName = username,
                Address = ""
            };
            user.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CustomerStatus.New.ToString();
            var status = await _userManager.CreateAsync(user, request.Password);
            if (!status.Succeeded)
            {
                var errorApi = new ApiErrorResult<bool>("Lỗi đăng kí");
                errorApi.ValidationErrors = new List<string>();
                foreach (var item in status.Errors)
                {
                    errorApi.ValidationErrors.Add(item.Description);
                }
                return errorApi;
            }


            var customerRole = new AppRole()
            {
                Name = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Customer,
                Description = "Khách hàng mặc định"
            };
            var role = await _roleManager.FindByNameAsync(customerRole.Name);
            if (role == null)
            {
                var statusCreateRole = await _roleManager.CreateAsync(customerRole);
                if (!status.Succeeded)
                {
                    return new ApiErrorResult<bool>("Lỗi hệ thống, không thể tạo role vui lòng thử lại");
                }
            }
           
            var statusAddRole = await _userManager.AddToRoleAsync(user, customerRole.Name);
            if (!statusAddRole.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống, không thể thêm role vào tải khoản vui lòng thử lại");
            }
            return new ApiSuccessResult<bool>(true, "Đăng kí thành công");
        }

        public async Task<ApiResult<bool>> UpdateCustomerAccount(UpdateCustomerRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.CustomerId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Khách hàng không tồn tại");
            }
            if (!DiamondLuxurySolution.Utilities.Helper.CheckValidInput.IsValidEmail(request.Email.Trim()))
            {
                return new ApiErrorResult<bool>("Email không hợp lệ");
            }
            if (DiamondLuxurySolution.Utilities.Helper.CheckValidInput.ContainsLetters(request.PhoneNumber.Trim()))
            {
                return new ApiErrorResult<bool>("Số điện thoại không hợp lệ");
            }
            if (!DiamondLuxurySolution.Utilities.Helper.CheckValidInput.ValidLenghPhoneNumber(request.PhoneNumber.Trim()))
            {
                return new ApiErrorResult<bool>("Số điện thoại không hợp lệ");
            }
            user.PhoneNumber = request.PhoneNumber.Trim();
            user.Fullname = request.FullName.Trim();
            user.Dob = request.Dob;
            user.Email = request.Email.Trim();
            user.Status = request.Status.Trim();
            
            var statusUser = await _userManager.UpdateAsync(user);
            if (!statusUser.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống, cập nhật thông tin thất bại vui lòng thử lại");
            }
            return new ApiSuccessResult<bool>("Cập nhật thông tin thành công");

        }

        public async Task<ApiResult<PageResult<CustomerVm>>> ViewCustomerPagination(ViewCustomerPaginationRequest request)
        {
            var listUser = _context.Users.AsQueryable();
            if (request.Keyword != null)
            {
                listUser = listUser.Where(x => x.Fullname.Contains(request.Keyword)
                || x.Email.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword));
            }
            listUser = listUser.OrderBy(x => x.UserName);
            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listUser.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();



            var listResult = new List<CustomerVm>();
            foreach (var item in listPaging)
            {
                var user = new CustomerVm()
                {
                    CustomerId = item.Id,
                    FullName = item.Fullname,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email,
                    Dob = (DateTime)item.Dob,
                    Status = item.Status
                };
                var appUser = await _userManager.FindByIdAsync(item.Id.ToString());
                var roles = await _userManager.GetRolesAsync(appUser);
                if (roles.Count>0)
                {
                    user.ListRoleName = new List<string>();
                    foreach (var role in roles)
                    {
                        user.ListRoleName.Add(role);
                    }
                    listResult.Add(user);
                }
                else
                {
                    listResult.Add(user);

                }

            }

            var result = new PageResult<CustomerVm>()
            {
                Items = listResult,
                TotalRecords = listUser.Count(),
                PageIndex = pageIndex,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE
            };
            var apiSuccess = new ApiSuccessResult<PageResult<CustomerVm>>(result, "Success");
            return apiSuccess;
        }
    }
}
