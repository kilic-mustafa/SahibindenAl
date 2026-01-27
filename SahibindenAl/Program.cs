using SahibindenAl.Models;
using System.Globalization;
using SahibindenAl.Service;
using SahibindenAl.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SahibindenAl.Data.AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAdvertService, AdvertService>();
builder.Services.AddScoped<IAdvertRepository, AdvertRepository>();
builder.Services.AddScoped<ISavedSearchService, SavedSearchService>();
builder.Services.AddScoped<ISavedSearchRepository, SavedSearchRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 3;
    })
    .AddEntityFrameworkStores<SahibindenAl.Data.AppDbContext>();

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("en-US") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
