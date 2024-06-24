
using DiamondLuxurySolution.WebApp.Service.Contact;

using DiamondLuxurySolution.Application.Repository.User;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.WebApp.Models;
using DiamondLuxurySolution.WebApp.Service.Account;

using DiamondLuxurySolution.WebApp.Service.GemPriceList;
using DiamondLuxurySolution.WebApp.Service.KnowledgeNews;
using DiamondLuxurySolution.WebApp.Service.Order;
using DiamondLuxurySolution.WebApp.Service.Product;
using DiamondLuxurySolution.WebApp.Service.Slide;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DiamondLuxurySolution.WebApp.Service.Customer;
using DiamondLuxurySolution.WebApp.Service.Payment;
using DiamondLuxurySolution.WebApp.Service.Promotion;
using DiamondLuxurySolution.AdminCrewApp.Service.Category;
using DiamondLuxurySolution.WebApp.Service.Collection;


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
builder.Services.AddTransient<ICustomerApiService, CustomerApiService>();
builder.Services.AddTransient<IPaymentApiService, PaymentApiService>();
builder.Services.AddTransient<IPromotionApiService, PromotionApiService>();


builder.Services.AddTransient<ISlideApiService, SlideApiService>();
builder.Services.AddTransient<IGemPriceListApiService, GemPriceListApiService>();
builder.Services.AddTransient<IKnowLedgeNewsApiService, KnowledgeNewsApiService>();
builder.Services.AddTransient<ICategoryApiService, CategoryApiService>();
builder.Services.AddTransient<IContactApiService, ContactApiService>();
builder.Services.AddTransient<ICollectionApiService, CollectionApiService>();

builder.Services.AddTransient<IProductApiService, ProductApiService>();



builder.Services.AddIdentity<AppUser, AppRole>()
.AddEntityFrameworkStores<LuxuryDiamondShopContext>().AddEntityFrameworkStores<LuxuryDiamondShopContext>()
.AddSignInManager<SignInManager<AppUser>>()
        .AddDefaultTokenProviders();



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1); // Set session timeout
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
CartSessionHelper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
