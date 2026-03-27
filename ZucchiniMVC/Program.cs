using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zucchinimvc.Data;
using Zucchinimvc.Models;
using Zucchinimvc.Models.Entities;
using Zucchinimvc.Repositories;
using Zucchinimvc.Services;
using Zucchinimvc.Services.API;
using Zucchinimvc.Services.Emails;
using Zucchinimvc.Services.Subscriptions;
using Zucchinimvc.Services.Users;


var builder = WebApplication.CreateBuilder(args);

// logger
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddRazorPages();
builder.Services.AddIdentity<User, Roles>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// Repositories
builder.Services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

builder.Services.AddSingleton<IHistoryRepository<WeatherHistoryEntity>>(sp =>
    new HistoryRepository<WeatherHistoryEntity>(
        sp.GetRequiredService<IConfiguration>(),
        sp.GetRequiredService<ILogger<HistoryRepository<WeatherHistoryEntity>>>(),
        "ExternalApiHistory"
    )); 



// missing Entity CurrencyHistoryEntity
/*builder.Services.AddSingleton<IHistoryRepository<CurrencyHistoryEntity>>(sp =>
    new HistoryRepository<CurrencyHistoryEntity>(
        sp.GetRequiredService<IConfiguration>(),
        sp.GetRequiredService<ILogger<HistoryRepository<CurrencyHistoryEntity>>>(),
        "ExternalApiHistory"
    ));*/

// Services
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IEmailSender<User>, EmailSender>();
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSender>();
builder.Services.AddHttpClient<WeatherService>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
