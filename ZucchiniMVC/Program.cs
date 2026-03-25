using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zucchinimvc.Data;
using Zucchinimvc.Models;
using Zucchinimvc.Repositories;
using Zucchinimvc.Services;


var builder = WebApplication.CreateBuilder(args);

// logger
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<WeatherService>();

builder.Services.AddRazorPages();
builder.Services.AddIdentity<User, Roles>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Repositories
builder.Services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

// missing Entity "WeatherHistoryEntity"
/*builder.Services.AddSingleton<IHistoryRepository<WeatherHistoryEntity>>(sp =>
    new HistoryRepository<WeatherHistoryEntity>(
        sp.GetRequiredService<IConfiguration>(),
        sp.GetRequiredService<ILogger<HistoryRepository<WeatherHistoryEntity>>>(),
        "ExternalApiHistory"
    ));
*/

// missing Entity CurrencyHistoryEntity
/*builder.Services.AddSingleton<IHistoryRepository<CurrencyHistoryEntity>>(sp =>
    new HistoryRepository<CurrencyHistoryEntity>(
        sp.GetRequiredService<IConfiguration>(),
        sp.GetRequiredService<ILogger<HistoryRepository<CurrencyHistoryEntity>>>(),
        "ExternalApiHistory"
    ));*/

// Services
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
