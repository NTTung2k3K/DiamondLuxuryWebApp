using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using DiamondLuxurySolution.AdminCrewApp.Service.Login;
using Azure.Core;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly ILoginApiService _loginApiService;

        public LoginController( IConfiguration configuration,ILoginApiService loginApiService)
        {
            _configuration = configuration;
            _loginApiService = loginApiService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(LoginStaffRequest request)
        {
           

            var apiResult = await _loginApiService.LoginStaff(request);

            if (apiResult.IsSuccessed)
            {
                var userPrincipal = this.ValidateToken(apiResult.ResultObj);
                var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = false
                };
                HttpContext.Session.SetString("token", apiResult.ResultObj);
                await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            userPrincipal,
                            authProperties);

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            var apiResult = await _loginApiService.ForgotpasswordStaffSendCode(Email);
            TempData["Code"] = apiResult.ResultObj.ToString();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> VerifyCode()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyCode(string code)
        {
            if (TempData["Code"].ToString().Equals(code.ToString()))
            {
                return RedirectToAction("ChangePaassword", "Login");
            }
            ViewBag.Error = "Sai mã xác thực";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ForgotPasswordStaffChangeRequest request)
        {
            var apiResult = await _loginApiService.ForgotpassworStaffdChange(request);
            return View();
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Jwt:Audience"];
            validationParameters.ValidIssuer = _configuration["Jwt:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }


        



    }

}
