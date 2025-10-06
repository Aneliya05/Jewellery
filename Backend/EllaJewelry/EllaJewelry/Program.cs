using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EllaJewelry.Infrastructure.Data;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using EllaJewelry.Infrastructure.Data.Entities;
using EllaJewelry.Core.DbServices;
using EllaJewelry.Core.Contracts;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("EllaJewelryDbContextConnection");
builder.Services.AddDbContext<EllaJewelryDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<EllaJewelryDbContext>();


//builder.Services.AddAuthentication()
//    .AddGoogle(options =>
//    {
//        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//    })
//    .AddFacebook(options =>
//    {
//        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
//        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
//    });


builder.Services.AddScoped<IUser, UserServices>();
builder.Services.AddScoped<ICategory, CategoryServices>();
builder.Services.AddScoped<IElement, ElementServices>();
builder.Services.AddScoped<IImage, ImageServices>();
builder.Services.AddScoped<IOrder, OrderServices>();
builder.Services.AddScoped<IProduct, ProductServices>();
builder.Services.AddScoped<IService, ServiceServices>();
builder.Services.AddScoped<IJewellery, JewelleryServices>();
////builder.Services.AddScoped<IEmailSender, 
///EmailSender>();


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout = new LockoutOptions { AllowedForNewUsers = true, DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10), MaxFailedAccessAttempts = 5 };
})
        .AddEntityFrameworkStores<EllaJewelryDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();
//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<YourPlaceDbContext>();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Idenity/Account/AccessDenied";
    options.SlidingExpiration = true;
});



var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var dbContext = services.GetRequiredService<EllaJewelryDbContext>();
//    dbContext.Database.Migrate();
//}

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

//IMPORTANT!!!
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    // Your custom endpoints before MapRazorPages

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Map Razor Pages (for Identity)
    endpoints.MapRazorPages();

    // Your custom endpoints after MapRazorPages
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();