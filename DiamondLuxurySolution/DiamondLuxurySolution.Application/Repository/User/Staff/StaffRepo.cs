using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using Microsoft.AspNetCore.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.User.Staff
{
    public class StaffRepo : IStaffRepo
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly LuxuryDiamondShopContext _context;

        public StaffRepo(LuxuryDiamondShopContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<ApiResult<bool>> ChangePasswordStaff(ChangePasswordStaffRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.StaffId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Nhân viên không tồn tại");
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

        public async Task<ApiResult<bool>> DeleteStaff(Guid StaffId)
        {
            var user = await _userManager.FindByIdAsync(StaffId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Nhân viên không tồn tại");
            }
            user.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.StaffStatus.Terminated.ToString();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<StaffVm>> GetStaffById(Guid StaffId)
        {
            var user = await _userManager.FindByIdAsync(StaffId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<StaffVm>("Nhân viên không tồn tại");
            }
            var staffVm = new StaffVm()
            {
                StaffId = user.Id,
                Dob = (DateTime)user.Dob,
                FullName = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                CitizenIDCard = user.CitizenIDCard,
                Image = user.Image,
                Address = user.Address
            };

            var listRoleOfUser = await _userManager.GetRolesAsync(user);

            if (listRoleOfUser.Count > 0)
            {
                staffVm.ListRoleName = new List<string>();
                foreach (var role in listRoleOfUser)
                {
                    staffVm.ListRoleName.Add(role);
                }
            }
            return new ApiSuccessResult<StaffVm>(staffVm, "Success");
        }

        public async Task<ApiResult<bool>> LoginStaff(LoginStaffRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName.Trim()) || string.IsNullOrEmpty(request.Password.Trim()))
            {
                return new ApiErrorResult<bool>("Tải khoản hoặc mật khẩu không đúng");
            }

            var user = await _userManager.FindByNameAsync(request.UserName.Trim());
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

        public async Task<ApiResult<bool>> RegisterStaffAccount(CreateStaffAccountRequest request)
        {
            if (!request.Password.Equals(request.ConfirmPassword))
            {
                return new ApiErrorResult<bool>("Password không trùng khớp");
            }
            if (!DiamondLuxurySolution.Utilities.Helper.CheckValidInput.IsValidEmail(request.Email))
            {
                return new ApiErrorResult<bool>("Email không hợp lệ");
            }
            if (DiamondLuxurySolution.Utilities.Helper.CheckValidInput.ContainsLetters(request.PhoneNumber))
            {
                return new ApiErrorResult<bool>("Số điện thoại không hợp lệ");
            }
            if (!DiamondLuxurySolution.Utilities.Helper.CheckValidInput.ValidLenghPhoneNumber(request.PhoneNumber))
            {
                return new ApiErrorResult<bool>("Số điện thoại không hợp lệ");
            }


            var userFindByUsername = await _userManager.FindByNameAsync(request.Username);
            if (userFindByUsername != null)
            {
                return new ApiErrorResult<bool>("Username đã tồn tại");
            }

            var userFindByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userFindByEmail != null)
            {
                return new ApiErrorResult<bool>("Email đã tồn tại");
            }



            var user = new AppUser()
            {
                Fullname = request.FullName.Trim(),
                Email = request.Email.Trim(),
                Dob = request.Dob,
                PhoneNumber = request.PhoneNumber.Trim(),
                UserName = request.Username.Trim(),
                Status = request.Status.Trim()

            };

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


            foreach (var roleId in request.RoleId)
            {
                var roleFindById = await _roleManager.FindByIdAsync(roleId.ToString());
                if (roleFindById == null) return new ApiErrorResult<bool>("Role không tồn tại");
                await _userManager.AddToRoleAsync(user, roleFindById.Name);
            }



            return new ApiSuccessResult<bool>(true, "Đăng kí thành công");
        }

        public async Task<ApiResult<bool>> UpdateStaffAccount(UpdateStaffAccountRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.StaffId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Nhân viên không tồn tại");
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
            user.CitizenIDCard = request.CitizenIDCard.Trim();

            if (request.Image != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Image);
                user.Image = firebaseUrl;
            }

            var statusUser = await _userManager.UpdateAsync(user);
            if (!statusUser.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống, cập nhật thông tin thất bại vui lòng thử lại");
            }
            return new ApiSuccessResult<bool>("Cập nhật thông tin thành công");
        }

        public async Task<ApiResult<PageResult<StaffVm>>> ViewStaffPagination(ViewStaffPaginationRequest request)
        {
            var listUser = _context.Users.AsQueryable();
            if (request.Keyword != null)
            {
                listUser = listUser.Where(x => x.Fullname.Contains(request.Keyword)
                || x.Email.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword) || x.CitizenIDCard.Contains(request.Keyword));
            }
            listUser = listUser.OrderBy(x => x.Fullname);
            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listUser.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listResult = new List<StaffVm>();
            foreach (var item in listPaging)
            {
                var user = new StaffVm()
                {
                    StaffId = item.Id,
                    FullName = item.Fullname,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email,
                    Dob = (DateTime)item.Dob,
                    Status = item.Status,
                    CitizenIDCard = item.CitizenIDCard,
                    Address = item.Address,
                    Image = item.Image,
                };
                var appUser = await _userManager.FindByIdAsync(item.Id.ToString());
                var roles = await _userManager.GetRolesAsync(appUser);
                if (roles.Count > 0)
                {
                    user.ListRoleName = new List<string>();
                    foreach (var role in roles)
                    {
                        user.ListRoleName.Add(role);
                    }
                    listResult.Add(user);
                }
                listResult.Add(user);

            }

            var result = new PageResult<StaffVm>()
            {
                Items = listResult,
                TotalRecords = listUser.Count(),
                PageIndex = pageIndex,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE
            };
            return new ApiSuccessResult<PageResult<StaffVm>>(result, "Success"); ;
        }
    }
}
