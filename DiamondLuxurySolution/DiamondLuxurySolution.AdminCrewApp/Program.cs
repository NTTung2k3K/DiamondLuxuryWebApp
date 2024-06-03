using DiamondLuxurySolution.AdminCrewApp.Service.Customer;
using DiamondLuxurySolution.AdminCrewApp.Service.Category;
using DiamondLuxurySolution.AdminCrewApp.Service.IInspectionCertificate;
using DiamondLuxurySolution.AdminCrewApp.Service.Contact;
using DiamondLuxurySolution.AdminCrewApp.Service.Platform;
using DiamondLuxurySolution.AdminCrewApp.Service.Role;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using DiamondLuxurySolution.AdminCrewApp.Service.Payment;
using DiamondLuxurySolution.AdminCrewApp.Service.Gem;
using DiamondLuxurySolution.AdminCrewApp.Service.Material;
using DiamondLuxurySolution.AdminCrewApp.Service.Slide;
using DiamondLuxurySolution.AdminCrewApp.Service.Frame;
using DiamondLuxurySolution.AdminCrewApp.Service.Discount;
using DiamondLuxurySolution.AdminCrewApp.Service.Login;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DiamondLuxurySolution.AdminCrewApp.Service.About;
using DiamondLuxurySolution.AdminCrewApp.Service.News;
using DiamondLuxurySolution.AdminCrewApp.Service.KnowledgeNewsCategoty;
using DiamondLuxurySolution.AdminCrewApp.Service.KnowledgeNewsCategory;
using DiamondLuxurySolution.AdminCrewApp.Service.Product;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ILoginApiService, LoginApiService>();
builder.Services.AddTransient<INewsApiService, NewsApiService>();
builder.Services.AddTransient<IProductApiService, ProductApiService>();

builder.Services.AddTransient<IRoleApiService, RoleApiService>();
builder.Services.AddTransient<IStaffApiService, StaffApiService>();
builder.Services.AddTransient<ICustomerApiService, CustomerApiService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IPlatformApiService, PlatformApiService>();
builder.Services.AddTransient<IFrameApiService, FrameApiService>();
builder.Services.AddTransient<IPaymentApiService, PaymentApiService>();
builder.Services.AddTransient<IDiscountApiService, DiscountApiService>();
builder.Services.AddTransient<IInspectionCertificateApiService, InspectionCertificateApiService>();
builder.Services.AddTransient<IContactApiService, ContactApiService>();
builder.Services.AddTransient<IGemApiService, GemApiService>();
builder.Services.AddTransient<IMaterialApiService, MaterialApiService>();
builder.Services.AddTransient<ISlideApiService, SlideApiService>();
builder.Services.AddTransient<IAboutApiService, AboutApiService>();
builder.Services.AddTransient<IKnowledgeNewsCategoryApiService, KnowledgeNewsCategoryApiService>();
builder.Services.AddTransient<ICategoryApiService, CategoryApiService>();

builder.Services.AddDbContext<LuxuryDiamondShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("eShopSolutionDb"));
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index/";
        options.AccessDeniedPath = "/Login/Forbidden/";
    });
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});
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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
