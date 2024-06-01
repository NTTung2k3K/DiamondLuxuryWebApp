using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.Utilities.Helper;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
namespace DiamondLuxurySolution.Application.Repository.User.Staff
{
    public class StaffRepo : IStaffRepo
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly LuxuryDiamondShopContext _context;
        private readonly IConfiguration _configuarion;

        public StaffRepo( LuxuryDiamondShopContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuarion = configuration;
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
            var status =  await _userManager.DeleteAsync(user);
            if (!status.Succeeded)
            {
                return new ApiErrorResult<bool>("Không thể xóa nhân viên");

            }
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
                Dob = (DateTime)(user.Dob ?? DateTime.MinValue),
                FullName = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                CitizenIDCard = user.CitizenIDCard,
                Image = user.Image,
                Address = user.Address,
                Status = user.Status,
                Username = user.UserName
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

        public async Task<ApiResult<string>> LoginStaff(LoginStaffRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName.Trim()) || string.IsNullOrEmpty(request.Password.Trim()))
            {
                return new ApiErrorResult<string>("Tải khoản hoặc mật khẩu không đúng");
            }

            var user = await _userManager.FindByNameAsync(request.UserName.Trim());
            if (user == null)
            {
                return new ApiErrorResult<string>("Tải khoản hoặc mật khẩu không đúng");
            }
            var userPasswordConfirm = await _userManager.CheckPasswordAsync(user, request.Password.Trim());
            if (userPasswordConfirm == false)
            {
                return new ApiErrorResult<string>("Tải khoản hoặc mật khẩu không đúng");
            }
            var authClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Email,user.Email),
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, item.ToString()));
            }
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuarion["Jwt:Key"]));
            var token = new JwtSecurityToken(
                    issuer: _configuarion["Jwt:Issuer"],
                    audience: _configuarion["Jwt:Audience"],
                    claims: authClaim,
                    expires: DateTime.Now.AddMinutes(3),
                    signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha512Signature)
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var ApiSuccess = new ApiSuccessResult<string>(tokenString, "Success");

            return ApiSuccess;
        }

        public async Task<ApiResult<bool>> RegisterStaffAccount(CreateStaffAccountRequest request)
        {
            List<string> errorList = new List<string>();
            if (!request.Password.Equals(request.ConfirmPassword))
            {
                errorList.Add("Password không trùng khớp");
            }
            
            if (!DiamondLuxurySolution.Utilities.Helper.CheckValidInput.ValidPhoneNumber(request.PhoneNumber))
            {
                errorList.Add("Số điện thoại không hợp lệ");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
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
                Dob = request.Dob != null ? request.Dob : null,
                PhoneNumber = request.PhoneNumber.Trim(),
                UserName = request.Username.Trim(),
                Status = request.Status.Trim(),
                CitizenIDCard = request.CitizenIDCard,
                Address = request.Address.Trim(),
            };
            if(request.Image != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Image);
                user.Image = firebaseUrl;
            }


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
                if (roleFindById.Name == DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.DeliveryStaff)
                {
                    user.ShipStatus = DiamondLuxurySolution.Utilities.Constants.Systemconstant.ShiperStatus.Waiting.ToString();
                  await  _userManager.UpdateAsync(user);
                }

            }



            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> UpdateStaffAccount(UpdateStaffAccountRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.StaffId.ToString());
            var errorList = new List<string>();
            if (user == null)
            {
                return new ApiErrorResult<bool>("Không hợp lệ", new List<string>() { "Username không tồn tại" });
            }
            if (!DiamondLuxurySolution.Utilities.Helper.CheckValidInput.IsValidEmail(request.Email.Trim()))
            {
                errorList.Add("Email không hợp lệ");
            }
            /*if (DiamondLuxurySolution.Utilities.Helper.CheckValidInput.ContainsLetters(request.PhoneNumber.Trim()))
            {
                errorList.Add("Số điện thoại không hợp lệ");
            }
            if (!DiamondLuxurySolution.Utilities.Helper.CheckValidInput.ValidPhoneNumber(request.PhoneNumber.Trim()))
            {
                errorList.Add("Số điện thoại không hợp lệ");
            }*/

            #region Check lỗi phoneNumbers
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                errorList.Add("Vui lòng nhập số điện thoại");
            }
            else
            {
                if (!Regex.IsMatch(request.PhoneNumber, "^(09|03|07|08|05)[0-9]{8,9}$"))
                {
                    errorList.Add("Số điện thoại không hợp lệ");
                }
            }
            #endregion End 


            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }

            user.PhoneNumber = request.PhoneNumber.Trim();
            user.Fullname = request.FullName.Trim();
            user.Dob = request.Dob != null ? request.Dob : null;

            user.Email = request.Email.Trim();
            user.CitizenIDCard = request.CitizenIDCard.Trim();

            user.Address = request.Address;
            user.Status = request.Status;

            if (request.Image != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Image);
                user.Image = firebaseUrl;
            }

            if (request.RoleId.Count > 0)
            {


                var listRole = await _userManager.GetRolesAsync(user);
                foreach (var role in listRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
                foreach (var roleAdd in request.RoleId)
                {
                    var role = await _roleManager.FindByIdAsync(roleAdd.ToString());
                    if (role == null) return new ApiErrorResult<bool>("Role is not exited!");
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }
            else
            {
                var listRole = await _userManager.GetRolesAsync(user);
                foreach (var role in listRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
            }


            var statusUser = await _userManager.UpdateAsync(user);
            if (!statusUser.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống, cập nhật thông tin thất bại vui lòng thử lại");
            }
            return new ApiSuccessResult<bool>("Cập nhật thông tin thành công");
        }

        public async Task<ApiResult<PageResult<StaffVm>>> ViewAdminPagination(ViewStaffPaginationCommonRequest request)
        {
            var users = await _userManager.Users.ToListAsync();
            var customers = new List<AppUser>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin))
                {
                    customers.Add(user);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                customers = customers.Where(x => x.Fullname.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.Email.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.PhoneNumber.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.CitizenIDCard.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            customers = customers.OrderBy(x => x.Fullname).ToList();
            int pageIndex = request.pageIndex ?? 1;
            int pageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE;

            var listPaging = customers.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var listResult = new List<StaffVm>();
            foreach (var item in listPaging)
            {
                var user = new StaffVm()
                {
                    StaffId = item.Id,
                    FullName = item.Fullname,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email,
                    Dob = (DateTime)(item.Dob ?? DateTime.MinValue),
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
                else
                {
                    listResult.Add(user);
                }
            }
            var result = new PageResult<StaffVm>()
            {
                Items = listResult,
                TotalRecords = customers.Count(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return new ApiSuccessResult<PageResult<StaffVm>>(result, "Success");
        }



        public async Task<ApiResult<PageResult<StaffVm>>> ViewDeliveryStaffPagination(ViewStaffPaginationCommonRequest request)
        {
            var users = await _userManager.Users.ToListAsync();
            var customers = new List<AppUser>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.DeliveryStaff))
                {
                    customers.Add(user);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                customers = customers.Where(x => x.Fullname.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.Email.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.PhoneNumber.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.CitizenIDCard.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            customers = customers.OrderBy(x => x.Fullname).ToList();
            int pageIndex = request.pageIndex ?? 1;
            int pageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE;

            var listPaging = customers.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var listResult = new List<StaffVm>();
            foreach (var item in listPaging)
            {
                var user = new StaffVm()
                {
                    StaffId = item.Id,
                    FullName = item.Fullname,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email,
                    Dob = (DateTime)(item.Dob ?? DateTime.MinValue),
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
                else
                {
                    listResult.Add(user);
                }
            }
            var result = new PageResult<StaffVm>()
            {
                Items = listResult,
                TotalRecords = customers.Count(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return new ApiSuccessResult<PageResult<StaffVm>>(result, "Success");
        }

        public async Task<ApiResult<PageResult<StaffVm>>> ViewManagerPagination(ViewStaffPaginationCommonRequest request)
        {
            var users = await _userManager.Users.ToListAsync();
            var customers = new List<AppUser>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager))
                {
                    customers.Add(user);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                customers = customers.Where(x => x.Fullname.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.Email.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.PhoneNumber.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.CitizenIDCard.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            customers = customers.OrderBy(x => x.Fullname).ToList();
            int pageIndex = request.pageIndex ?? 1;
            int pageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE;

            var listPaging = customers.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var listResult = new List<StaffVm>();
            foreach (var item in listPaging)
            {
                var user = new StaffVm()
                {
                    StaffId = item.Id,
                    FullName = item.Fullname,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email,
                    Dob = (DateTime)(item.Dob ?? DateTime.MinValue),
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
                else
                {
                    listResult.Add(user);
                }
            }
            var result = new PageResult<StaffVm>()
            {
                Items = listResult,
                TotalRecords = customers.Count(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return new ApiSuccessResult<PageResult<StaffVm>>(result, "Success");
        }

        public async Task<ApiResult<PageResult<StaffVm>>> ViewSalesStaffPagination(ViewStaffPaginationCommonRequest request)
        {
            var users = await _userManager.Users.ToListAsync();
            var customers = new List<AppUser>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff))
                {
                    customers.Add(user);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                customers = customers.Where(x => x.Fullname.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.Email.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.PhoneNumber.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.CitizenIDCard.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            customers = customers.OrderBy(x => x.Fullname).ToList();
            int pageIndex = request.pageIndex ?? 1;
            int pageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE;

            var listPaging = customers.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var listResult = new List<StaffVm>();
            foreach (var item in listPaging)
            {
                var user = new StaffVm()
                {
                    StaffId = item.Id,
                    FullName = item.Fullname,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email,
                    Dob = (DateTime)(item.Dob ?? DateTime.MinValue),
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
                else
                {
                    listResult.Add(user);
                }
            }
            var result = new PageResult<StaffVm>()
            {
                Items = listResult,
                TotalRecords = customers.Count(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return new ApiSuccessResult<PageResult<StaffVm>>(result, "Success");
        }

        public async Task<ApiResult<PageResult<CustomerVm>>> ViewCustomerPagination(ViewStaffPaginationCommonRequest request)
        {
            var users = await _userManager.Users.ToListAsync();
            var customers = new List<AppUser>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Customer))
                {
                    customers.Add(user);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                customers = customers.Where(x => x.Fullname.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.Email.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.PhoneNumber.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                              || x.CitizenIDCard.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            customers = customers.OrderBy(x => x.Fullname).ToList();
            int pageIndex = request.pageIndex ?? 1;
            int pageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE;

            var listPaging = customers.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var listResult = new List<CustomerVm>();
            foreach (var item in listPaging)
            {
                var user = new CustomerVm()
                {
                    CustomerId = item.Id,
                    FullName = item.Fullname,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email,
                    Dob = (DateTime)(item.Dob ?? DateTime.MinValue),
                    Status = item.Status,
                    Address = item.Address,
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
                else
                {
                    listResult.Add(user);
                }
            }
            var result = new PageResult<CustomerVm>()
            {
                Items = listResult,
                TotalRecords = customers.Count(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return new ApiSuccessResult<PageResult<CustomerVm>>(result, "Success");
        }

        public async Task<ApiResult<string>> ForgotpasswordStaffSendCode(string Username)
        {
            var user = await _userManager.FindByNameAsync(Username.Trim().ToString());
            if (user == null)
            {
                return new ApiErrorResult<string>("Tài khoản không tồn tại");
            }
            Random rd = new Random();
            string code = ""+rd.Next(0,9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString();
            user.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CustomerStatus.ChangePasswordRequest.ToString();
            var statusUser = await _userManager.UpdateAsync(user);
            if (!statusUser.Succeeded)
            {
                return new ApiErrorResult<string>("Lỗi hệ thống, cập nhật thông tin thất bại vui lòng thử lại");
            }
            SendEmail(code,user.Email);

            return new ApiSuccessResult<string>(code, "Success");
        }
        public void SendEmail(string code, string toEmail)
        {
            // Correct relative path from current directory to the HTML file
            string relativePath = @"..\..\DiamondLuxurySolution\DiamondLuxurySolution.Utilities\FormSendEmail\SendCode.html";
            // Combine the relative path with the current directory to get the full path
            string path = Path.GetFullPath(relativePath);

            if (!System.IO.File.Exists(path))
            {
                return;
            }
            string contentCustomer = System.IO.File.ReadAllText(path);
            contentCustomer = contentCustomer.Replace("{{VerifyCode}}", code);
            DoingMail.SendMail("LuxuryDiamond", "Yêu cầu thay đổi mật khẩu", contentCustomer, toEmail);
        }


        public async Task<ApiResult<bool>> ForgotpassworStaffdChange(ForgotPasswordStaffChangeRequest request)
        {
            if (!request.NewPassword.Trim().Equals(request.ConfirmPassword.Trim()))
            {
                return new ApiErrorResult<bool>("Mật khẩu không trùng khớp");
            }
            var user = await _userManager.FindByNameAsync(request.Username.Trim().ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Email không tồn tại");
            }
            if (!user.Status.Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.StaffStatus.ChangePasswordRequest.ToString()))
            {
                return new ApiErrorResult<bool>("Không có yêu câu thay đổi mật khẩu");
            }
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                return new ApiErrorResult<bool>("Xóa mật khẩu cũ thất bại");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, request.NewPassword.Trim());
            if (!addPasswordResult.Succeeded)
            {
                var errorApi = new ApiErrorResult<bool>("Lỗi thông tin");
                errorApi.ValidationErrors = new List<string>();
                foreach (var item in addPasswordResult.Errors)
                {
                    errorApi.ValidationErrors.Add(item.Description);
                }
                return errorApi;
            }
            if (!addPasswordResult.Succeeded)
            {

                return new ApiErrorResult<bool>("Thêm mật khẩu mới thất bại");
            }
            user.LastChangePasswordTime = DateTime.Now;
            user.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CustomerStatus.Active.ToString();
            var statusUser = await _userManager.UpdateAsync(user);
            if (!statusUser.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống, cập nhật thông tin thất bại vui lòng thử lại");
            }

            return new ApiSuccessResult<bool>(true, "Thay đổi mật khẩu thành công");
        }

        public async Task<ApiResult<bool>> ChangeStatusCustomer(ChangeStatusCustomerRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Email không tồn tại");
            }
            user.Status = request.Status;
            var statusUser = await _userManager.UpdateAsync(user);
            if (!statusUser.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống, cập nhật thông tin thất bại vui lòng thử lại");
            }
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<Guid>> GetStaffByUsername(string Username)
        {
            var user = await _userManager.FindByNameAsync(Username.ToString());
            if (user == null)
            {
                return new ApiErrorResult<Guid>("Nhân viên không tồn tại");
            }

            return new ApiSuccessResult<Guid>(user.Id, "Success");
        }
    }
}
