using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Implementations;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Business.Services;
using ECommerce.Business.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using ECommerce.Business.Mapping;
using ECommerce.Business.Services.OrderFacade;
using ECommerce.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Razor View ve Controller desteði
builder.Services.AddControllersWithViews();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddViewLocalization();

// DbContext
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
//    sql =>sql.MigrationsHistoryTable("__EFMigrationsHistory", "a.digil"));


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "a.digil"));
});

// Repositories & UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAddressService, AddressManager>();
builder.Services.AddScoped<ICuponService, CuponManager>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ICartCookieService, CartCookieService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailManager>();
builder.Services.AddScoped<IPaymentService, PaymentManager>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IOrderFacadeService, OrderFacadeService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ICustomOrderService, CustomOrderService>();
builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddMemoryCache();
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();


// AutoMapper
// Fix for CS1503: Correcting the argument passed to AddAutoMapper  
//builder.Services.AddAutoMapper(cfg =>
//{
//    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
//});
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
    cfg.AddProfile<ProductProfile>();
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Error handling & HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

var supportedCultures = new[] { new CultureInfo("tr-TR") };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr-TR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseRouting();
app.UseMiddleware<MaintenanceMiddleware>();
app.UseAuthorization();

// MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
