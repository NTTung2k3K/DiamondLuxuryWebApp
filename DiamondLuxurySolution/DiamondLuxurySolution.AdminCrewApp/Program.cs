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
using DiamondLuxurySolution.AdminCrewApp.Service.SubGem;
using DiamondLuxurySolution.AdminCrewApp.Service.Material;
using DiamondLuxurySolution.AdminCrewApp.Service.Slide;
using DiamondLuxurySolution.AdminCrewApp.Service.About;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRoleApiService, RoleApiService>();
builder.Services.AddTransient<IStaffApiService, StaffApiService>();
builder.Services.AddTransient<ICustomerApiService, CustomerApiService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IPlatformApiService, PlatformApiService>();

builder.Services.AddTransient<IPaymentApiService, PaymentApiService>();

builder.Services.AddTransient<IInspectionCertificateApiService, InspectionCertificateApiService>();
builder.Services.AddTransient<IContactApiService, ContactApiService>();
builder.Services.AddTransient<IGemApiService, GemApiService>();
builder.Services.AddTransient<IMaterialApiService, MaterialApiService>();
builder.Services.AddTransient<ISlideApiService, SlideApiService>();
    builder.Services.AddTransient<IAboutApiService, AboutApiService>();

builder.Services.AddTransient<ICategoryApiService, CategoryApiService>();
builder.Services.AddTransient<IInspectionCertificateApiService, InspectionCertificateApiService>();
builder.Services.AddTransient<ISubGemApiService, SubGemApiService>();


builder.Services.AddDbContext<LuxuryDiamondShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("eShopSolutionDb"));
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
