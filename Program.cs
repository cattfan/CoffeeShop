using Coffeeshop.Data;
using Coffeeshop.Models.interfaces;
using Coffeeshop.Models.Interfaces;
using Coffeeshop.Models.Services;
using CoffeeShop.Models.Services;
using Microsoft.EntityFrameworkCore;
using CoffeeShopDbContext_CoffeeShop;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Đăng ký các repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Đăng ký ShoppingCartRepository và truyền IServiceProvider để GetCart có thể resolve IHttpContextAccessor
builder.Services.AddScoped<IShoppingCartRepository>(sp => ShoppingCartRepository.GetCart(sp));

builder.Services.AddScoped<CoffeeShop.Models.Interfaces.IOrderRepository, CoffeeShop.Models.Services.OrderRepository>();

builder.Services.AddDbContext<CoffeeshopDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("CoffeeShopDbContextConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Thêm hỗ trợ cho roles
    .AddEntityFrameworkStores<CoffeeshopDbContext>();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Data_>();
builder.Services.AddHttpContextAccessor();

// Cấu hình Session
builder.Services.AddDistributedMemoryCache(); // Cần thiết cho AddSession nếu không dùng external store
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session hết hạn
    options.Cookie.HttpOnly = true; // Cookie chỉ được truy cập bởi server
    options.Cookie.IsEssential = true; // Đánh dấu cookie là cần thiết cho ứng dụng hoạt động
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

app.UseSession(); // Enable session support
app.UseRouting();

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();