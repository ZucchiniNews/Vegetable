using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.ApiClients.WeatherClient;
using Infrastructure.ApiClients.CmsClient;
using Infrastructure.ApiClients.AzureTableClient;
using Infrastructure.Config;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Application.Interfaces;
using Application.Services.Emails;
using Application.Services.Subscriptions;
using Application.Services.UsersService;
using Application.Services.CMS;
using Application.Services.Weather;
using Application.Services.Logger;

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


// --- Configurations ---
builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherApi"));
builder.Services.Configure<CmsSettings>(builder.Configuration.GetSection("StrapiSettings"));

// --- Http Clients (Typed) ---
// These handle the BaseUrl and specific API logic
builder.Services.AddHttpClient<WeatherClient>();
builder.Services.AddHttpClient<CmsClient>();
// --- Provider client for Azure Table Storage ---
builder.Services.AddSingleton<IAzureTableClient, AzureTableClient>();
// --- Repositories ---
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ICmsRepository, CmsRepository>();

// Weather Repository
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped(sp =>
{
    var provider = sp.GetRequiredService<IAzureTableClient>();
    return provider.GetClient("ExternalApiHistory");
});
builder.Services.AddScoped<IHistoryRepository<WeatherHistory>, WeatherHistoryRepository>();

// --- Services ---
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<ICmsService, CmsService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Email Services
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IEmailSender<User>, EmailSender>();
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSender>();

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
