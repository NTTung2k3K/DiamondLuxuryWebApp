using DiamondLuxurySolution.Application.Repository;
using DiamondLuxurySolution.Application.Repository.About;
using DiamondLuxurySolution.Application.Repository.Discount;
using DiamondLuxurySolution.Application.Repository.Gem;
using DiamondLuxurySolution.Application.Repository.InspectionCertificate;
using DiamondLuxurySolution.Application.Repository.Material;
using DiamondLuxurySolution.Application.Repository.GemPriceList;
using DiamondLuxurySolution.Application.Repository.News;
using DiamondLuxurySolution.Application.Repository.Platform;
using DiamondLuxurySolution.Application.Repository.Promotion;
using DiamondLuxurySolution.Application.Repository.Slide;
using DiamondLuxurySolution.Application.Repository.User.Customer;
using DiamondLuxurySolution.Application.Repository.User.Staff;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DiamondLuxurySolution.Application.Repository.Order;
using DiamondLuxurySolution.Application.Repository.Product;
using DiamondLuxurySolution.Application.Repository.KnowledgeNewCatagory;
using DiamondLuxurySolution.Application.Repository.Category;
using DiamondLuxurySolution.Application.Repository.Collection;
using DiamondLuxurySolution.Application.Repository.Warranty;
using DiamondLuxurySolution.Application.Repository.KnowledgeNews;
using DiamondLuxurySolution.Application.Repository.SubGem;
using DiamondLuxurySolution.Application.Repository.Payment;
using DiamondLuxurySolution.Application.Repository.Contact;
using DiamondLuxurySolution.Application.Repository.Frame;
using DiamondLuxurySolution.Application.Repository.Role;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DiamondLuxurySolution.Application.Repository.User;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<LuxuryDiamondShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LuxuryDiamondDb"));
});
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddTransient<IPlatformRepo, PlatformRepo>();
builder.Services.AddTransient<IAboutRepo, AboutRepo>();
builder.Services.AddTransient<ISlideRepo, SlideRepo>();
builder.Services.AddTransient<IRoleRepo, RoleRepo>();
builder.Services.AddTransient<ICustomerRepo, CustomerRepo>();
builder.Services.AddTransient<IStaffRepo, StaffRepo>();
builder.Services.AddTransient<IPromotionRepo, PromotionRepo>();
builder.Services.AddTransient<IDiscountRepo, DiscountRepo>();
builder.Services.AddTransient<IGemRepo, GemRepo>();
builder.Services.AddTransient<ISubGemRepo, SubGemRepo>();
builder.Services.AddTransient<IInspectionCertificateRepo, InspectionCertificateRepo>();
builder.Services.AddTransient<IMaterialRepo, MaterialRepo>();
builder.Services.AddTransient<INewsRepo, NewsRepo>();
builder.Services.AddTransient<IGemPriceListRepo, GemPriceListRepo>();
builder.Services.AddTransient<IOrderRepo, OrderRepo>();
builder.Services.AddTransient<IProductRepo, ProductRepo>();
builder.Services.AddTransient<IKnowledgeNewCatagoryRepo, KnowledgeNewCatagoryRepo>();
builder.Services.AddTransient<ICategoryRepo, CategoryRepo>();
builder.Services.AddTransient<ICollectionRepo, CollectionRepo>();
builder.Services.AddTransient<IWarrantyRepo, WarrantyRepo>();
builder.Services.AddTransient<IKnowledgeNewsRepo, KnowledgeNewsRepo>();
builder.Services.AddTransient<IPaymentRepo, PaymentRepo>();
builder.Services.AddTransient<IContactRepo, ContactRepo>();
builder.Services.AddTransient<IFrameRepo, FrameRepo>();
builder.Services.AddScoped<IRoleInitializer, RoleInitializer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<LuxuryDiamondShopContext>().AddEntityFrameworkStores<LuxuryDiamondShopContext>()
        .AddDefaultTokenProviders()
        .AddErrorDescriber<CustomIdentityErrorDescriber>();


var app = builder.Build();

app.UseCors(builder =>
       builder.WithOrigins("https://localhost:9002")  // Allow requests from this origin
              .AllowAnyMethod()                      // Allow any HTTP method
              .AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var roleInitializer = scope.ServiceProvider.GetRequiredService<IRoleInitializer>();
    roleInitializer.CreateDefaultRole().Wait();
    roleInitializer.CreateAdminAccount().Wait();
    roleInitializer.CreateManagerAccount().Wait();
    await roleInitializer.CreateCustomerAccount();
    roleInitializer.CreateSaleStaffAccount().Wait();
    roleInitializer.CreateShipperAccount().Wait();



}
app.Run();
