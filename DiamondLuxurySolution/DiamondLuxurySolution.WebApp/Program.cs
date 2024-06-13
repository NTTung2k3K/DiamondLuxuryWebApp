using DiamondLuxurySolution.WebApp.Service.GemPriceList;
using DiamondLuxurySolution.WebApp.Service.Slide;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<ISlideApiService,SlideApiService>();
builder.Services.AddTransient<IGemPriceListApiService, GemPriceListApiService>();

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleLogin:ClientID").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleLogin:ClientSecret").Value;
}).AddFacebook(options =>
{

    options.ClientId = builder.Configuration.GetSection("FacebookLogin:AppID").Value;
    options.ClientSecret = builder.Configuration.GetSection("FacebookLogin:AppSecret").Value;
    options.Scope.Remove("email");
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
