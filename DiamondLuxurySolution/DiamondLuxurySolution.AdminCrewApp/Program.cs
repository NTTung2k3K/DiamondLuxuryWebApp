<<<<<<< HEAD
=======

using DiamondLuxurySolution.AdminCrewApp.Service.Category;
>>>>>>> 5a3d0a77f6492a0e0a6e38f5df87b23f73c1b6b1
using DiamondLuxurySolution.AdminCrewApp.Service.IInspectionCertificate;
using DiamondLuxurySolution.AdminCrewApp.Service.Contact;
using DiamondLuxurySolution.AdminCrewApp.Service.Platform;
using DiamondLuxurySolution.AdminCrewApp.Service.Role;
using DiamondLuxurySolution.Data.EF;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using DiamondLuxurySolution.AdminCrewApp.Service.Material;
=======
using DiamondLuxurySolution.AdminCrewApp.Service.Slide;
>>>>>>> 5a3d0a77f6492a0e0a6e38f5df87b23f73c1b6b1

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRoleApiService, RoleApiService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IPlatformApiService, PlatformApiService>();
<<<<<<< HEAD
builder.Services.AddTransient<IInspectionCertificateApiService, InspectionCertificateApiService>();
builder.Services.AddTransient<IContactApiService, ContactApiService>();
builder.Services.AddTransient<IMaterialApiService, MaterialApiService>();

=======


builder.Services.AddTransient<ISlideApiService, SlideApiService>();

builder.Services.AddTransient<ICategoryApiService, CategoryApiService>();
builder.Services.AddTransient<IInspectionCertificateApiService, InspectionCertificateApiService>();

builder.Services.AddTransient<IContactApiService, ContactApiService>();
>>>>>>> 5a3d0a77f6492a0e0a6e38f5df87b23f73c1b6b1
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
