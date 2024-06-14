using DiamondLuxurySolution.Application.Repository.User;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.WebApp.Service.Account;
using DiamondLuxurySolution.WebApp.Service.GemPriceList;
using DiamondLuxurySolution.WebApp.Service.Order;
using DiamondLuxurySolution.WebApp.Service.Slide;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LuxuryDiamondShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LuxuryDiamondDb"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IAccountApiService, AccountApiService>();
builder.Services.AddTransient<IOrderApiService, OrderApiService>();

builder.Services.AddTransient<ISlideApiService, SlideApiService>();
builder.Services.AddTransient<IGemPriceListApiService, GemPriceListApiService>();


builder.Services.AddIdentity<AppUser, AppRole>()
.AddEntityFrameworkStores<LuxuryDiamondShopContext>().AddEntityFrameworkStores<LuxuryDiamondShopContext>()
.AddSignInManager<SignInManager<AppUser>>()
        .AddDefaultTokenProviders();



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleLogin:ClientID").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleLogin:ClientSecret").Value;
    options.AccessDeniedPath = "/Account/Login";
}).AddFacebook(options =>
{
    options.ClientId = builder.Configuration.GetSection("FacebookLogin:AppID").Value;
    options.ClientSecret = builder.Configuration.GetSection("FacebookLogin:AppSecret").Value;
    options.Scope.Remove("email");
    options.AccessDeniedPath = "/Account/Login";
});
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
