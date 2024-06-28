using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.WebApp.Service.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.WebApp.Models;
using Azure.Core;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;
using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Firebase.Auth;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using DiamondLuxurySolution.ViewModel.Models.Order;
using Microsoft.CodeAnalysis.Options;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountApiService _accountApiService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IAccountApiService accountApiService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _accountApiService = accountApiService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCustomerRequest request)
        {
            var apiResult = await _accountApiService.Login(request);

            if (apiResult.IsSuccessed)
            {
                var customer = await _accountApiService.GetCustomerByEmail(request.Email);
                if (customer == null)
                {
                    ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
                    return View();
                }

                if (request.RememberMe)
                {
                    var option = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(5)
                    };
                    Response.Cookies.Append("CustomerName", customer.ResultObj.FullName, option);
                    Response.Cookies.Append("CustomerId", customer.ResultObj.CustomerId.ToString(), option);
                    Response.Cookies.Append(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PLATFORM.ToString(), DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.DEFAULT_PLATFORM.ToString(), option);

                }
                HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PLATFORM, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.DEFAULT_PLATFORM);
                HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_NAME, customer.ResultObj.FullName);
                HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID, customer.ResultObj.CustomerId.ToString());
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCustomerAccountRequest request)
        {

            var status = await _accountApiService.Register(request);

            if (status is ApiErrorResult<bool> errorResult)
            {
                List<string> listError = new List<string>();

                if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                {
                    foreach (var error in errorResult.ValidationErrors)
                    {
                        listError.Add(error);
                    }
                }
                else if (status.Message != null)
                {
                    listError.Add(errorResult.Message);
                }
                ViewBag.Errors = listError;
                return View(request);
            }

            TempData["SuccessMsg"] = "Tạo mới thành công cho " + request.FullName;

            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        [InternalRedirect]
        public async Task<IActionResult> Profile()
        {


            string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID);
            Guid userId;


            Guid.TryParse(userIdString, out userId);
			// Chuyển đổi thành công
			var customer = await _userManager.FindByIdAsync(userIdString);
            ViewBag.CusPoint = customer.Point;
			var orderRequest = new ViewOrderRequest();
            orderRequest.CustomerId = userId;
            var listOrderRaw = await _accountApiService.GetListOrderOfCustomer(orderRequest);
            var listOrder = listOrderRaw.ResultObj;
            ViewBag.ListOrder = listOrder.Items.OrderByDescending(x => x.Datemodified).Take(8).ToList();

            var staff = await _accountApiService.GetCustomerById(userId);
            if (staff is ApiErrorResult<CustomerVm> errorResult)
            {
                return View(errorResult);
            }
            //Convert to UpdateCustomerRequest
            var updateCustomerRequest = new UpdateCustomerRequest()
            {
                CustomerId = staff.ResultObj.CustomerId,
                PhoneNumber = staff.ResultObj.PhoneNumber ?? null,
                Address = staff.ResultObj.Address ?? null,
                Dob = staff.ResultObj.Dob != DateTime.MinValue ? staff.ResultObj.Dob : DateTime.MinValue,
                FullName = staff.ResultObj.FullName ?? null,
                Email = staff.ResultObj.Email ?? null,
            };



            return View(updateCustomerRequest);

        }

        [HttpPost]
        public async Task<IActionResult> Profile(UpdateCustomerRequest request)
        {

            string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID);
            Guid userId;

            Guid.TryParse(userIdString, out userId);
			// Chuyển đổi thành công
			var customer = await _userManager.FindByIdAsync(userIdString);
			ViewBag.CusPoint = customer.Point;
			var orderRequest = new ViewOrderRequest();
            orderRequest.CustomerId = userId;

            var listOrderRaw = await _accountApiService.GetListOrderOfCustomer(orderRequest);
            var listOrder = listOrderRaw.ResultObj;
            ViewBag.ListOrder = listOrder.Items.OrderByDescending(x => x.Datemodified).Take(8).ToList();


            request.CustomerId = userId;
            var status = await _accountApiService.UpdateCustomerAccount(request);

            if (status is ApiErrorResult<bool> errorResult)
            {
                List<string> listError = new List<string>();

                if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                {
                    foreach (var error in errorResult.ValidationErrors)
                    {
                        listError.Add(error);
                    }
                }
                else if (status.Message != null)
                {
                    listError.Add(errorResult.Message);
                }
                ViewBag.Errors = listError;
                return View(request);
            }

            TempData["SuccessMsg"] = "Tạo mới thành công cho " + request.FullName;

            return RedirectToAction("Profile", "Account");
        }

        [HttpGet]
        public IActionResult ChangePasswordInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordInfo(ChangePasswordCustomerRequest request)
        {
            string userIdString = HttpContext.Session.GetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.USER_ID);
            Guid userId;

            Guid.TryParse(userIdString, out userId);
            // Chuyển đổi thành công
            request.CustomerId = userId;

            var status = await _accountApiService.ChangePasswordCustomer(request);

            if (status is ApiErrorResult<bool> errorResult)
            {
                List<string> listError = new List<string>();

                if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                {
                    foreach (var error in errorResult.ValidationErrors)
                    {
                        listError.Add(error);
                    }
                }
                else if (status.Message != null)
                {
                    listError.Add(errorResult.Message);
                }
                ViewBag.Errors = listError;
                return View(request);
            }

            TempData["SuccessMsg"] = "Tạo mới thành công";

            return RedirectToAction("Profile", "Account");
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_NAME);
            HttpContext.Session.Remove(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID);
            HttpContext.Session.Remove(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PLATFORM);

            Response.Cookies.Delete("CustomerName");
            Response.Cookies.Delete("CustomerId");
            Response.Cookies.Delete(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PLATFORM);


            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            var status = await _accountApiService.ForgotpasswordCustomerSendCode(Email);
            if (status is ApiErrorResult<string> errorResult)
            {
                List<string> listError = new List<string>();

                if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                {
                    foreach (var error in errorResult.ValidationErrors)
                    {
                        listError.Add(error);
                    }
                }
                else if (status.Message != null)
                {
                    listError.Add(errorResult.Message);
                }
                ViewBag.Errors = listError;
                ViewBag.Email = Email;

                return View();
            }
            HttpContext.Session.SetString("Code", status.ResultObj.ToString());
            HttpContext.Session.SetString("Username", Email.ToString());

            return RedirectToAction("VerifyCode", "Account");
        }
        [HttpGet]
        [InternalRedirect]

        public async Task<IActionResult> VerifyCode()
        {

            return View();
        }
        [HttpPost]
        [InternalRedirect]

        public async Task<IActionResult> VerifyCode(string code)
        {
            if (HttpContext.Session.GetString("Code").ToString().Equals(code.ToString()))
            {
                HttpContext.Session.Remove("Code");
                return RedirectToAction("ChangePassword", "Account");
            }
            ViewBag.Error = "Sai mã xác thực";
            return View((object)code);
        }

        [HttpGet]
        [InternalRedirect]

        public async Task<IActionResult> ChangePassword()
        {

            return View();
        }
        [HttpPost]
        [InternalRedirect]

        public async Task<IActionResult> ChangePassword(ForgotPasswordCustomerChangeRequest request)
        {
            request.Email = HttpContext.Session.GetString("Username").ToString();
            var status = await _accountApiService.ForgotpasswordCustomerChange(request);
            if (status is ApiErrorResult<bool> errorResult)
            {
                List<string> listError = new List<string>();

                if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                {
                    foreach (var error in errorResult.ValidationErrors)
                    {
                        listError.Add(error);
                    }
                }
                else if (status.Message != null)
                {
                    listError.Add(errorResult.Message);
                }
                ViewBag.Errors = listError;
                return View();

            }

            return RedirectToAction("Login", "Account");
        }


        public IActionResult LoginOnAnotherPlatform(string Provider)
        {

            if (HttpContext.Request.Query.ContainsKey("error"))
            {
                // Handle the error, e.g., redirect to login page with a custom message
                return RedirectToAction("Login", "Account", new { loginError = "External login cancelled by user." });
            }

            string redirectUrl = Url.Action("LoginOnAnotherPlatformResponse", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(Provider, redirectUrl);
            var result = new ChallengeResult(Provider, properties);
            return result;
        }

        public async Task<IActionResult> LoginOnAnotherPlatformResponse()
        {
            try
            {
                ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    // If there is no external login info, redirect to the home page
                    return RedirectToAction("Index", "Home");
                }

                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
                if (result.Succeeded)
                {
                    if (info.LoginProvider.Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.FACEBOOK_PLATFORM.ToString()))
                    {
                        var userExist = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey); ;

                        HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_NAME, userExist.Fullname);
                        HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID, userExist.Id.ToString());
                        HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PLATFORM, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.FACEBOOK_PLATFORM);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        string emailFromExternalProvider = info.Principal.FindFirst(ClaimTypes.Email)?.Value;
                        var userExist = await _userManager.FindByEmailAsync(emailFromExternalProvider);
                        HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PLATFORM, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.GOOGLE_PLATFORM);

                        HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_NAME, userExist.Fullname);
                        HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID, userExist.Id.ToString());

                        return RedirectToAction("Index", "Home");
                    }


                }
                else
                {
                    // If login fails, create a new user
                    Random rd = new Random();
                    string username;
                    AppUser user = null;
                    bool isSkip = false;
                    if (info.LoginProvider.Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.FACEBOOK_PLATFORM))
                    {
                        username = "FB" + string.Concat(Enumerable.Range(0, 6).Select(_ => rd.Next(0, 10).ToString()));
                        user = new AppUser()
                        {
                            Firstname = info.Principal.FindFirst(ClaimTypes.GivenName)?.Value,
                            Lastname = info.Principal.FindFirst(ClaimTypes.Surname)?.Value,
                            Fullname = info.Principal.FindFirst(ClaimTypes.Name)?.Value,
                            UserName = username,
                            DateCreated = DateTime.Now,
                            Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CustomerStatus.New.ToString(),
                            Point = 0,
                        };
						HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PLATFORM, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.FACEBOOK_PLATFORM);

					}
					else
                    {

                        var emailUser = info.Principal.FindFirst(ClaimTypes.Email)?.Value;
                        var userCheck = await _userManager.FindByEmailAsync(emailUser);
                        if (userCheck == null) 
                        {
							username = "GG" + string.Concat(Enumerable.Range(0, 6).Select(_ => rd.Next(0, 10).ToString()));
							user = new AppUser()
							{
								Firstname = info.Principal.FindFirst(ClaimTypes.GivenName)?.Value,
								Lastname = info.Principal.FindFirst(ClaimTypes.Surname)?.Value,
								Fullname = info.Principal.FindFirst(ClaimTypes.Name)?.Value,
								Email = info.Principal.FindFirst(ClaimTypes.Email)?.Value,
								UserName = username,
								DateCreated = DateTime.Now,
								Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CustomerStatus.New.ToString(),
								Point = 0,
							};
                        }
                        else
                        {
                            isSkip = true;
                            user = userCheck;
                        }
						HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PLATFORM, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.GOOGLE_PLATFORM);

					}

					if (isSkip == false)
                    {
						var createUserResult = await _userManager.CreateAsync(user);

						if (createUserResult.Succeeded)
						{
							var roleResult = await _userManager.AddToRoleAsync(user, DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Customer.ToString());
							var identResult = await _userManager.AddLoginAsync(user, info);
							if (identResult.Succeeded && roleResult.Succeeded)
							{
								await _signInManager.SignInAsync(user, false);

								HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_NAME, user.Fullname);
								HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID, user.Id.ToString());

								return RedirectToAction("Index", "Home");
							}
						}
                    }

					HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_NAME, user.Fullname);
					HttpContext.Session.SetString(DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.CUSTOMER_ID, user.Id.ToString());

					return RedirectToAction("Index", "Home");
                }
            }
            catch (AuthenticationFailureException ex)
            {

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet("ExternalLoginCallback", Name = "ExternalLoginCallback")]
        [AllowAnonymous]
        public Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            //Here we can retrieve the claims
            var result = HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return null;
        }
    }
}
