using Infrastrcture.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Logger;
using Zucchinimvc.Application.Services.CMS;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Infrastructure.ApiClients.WeatherClient;
using Zucchinimvc.Infrastructure.ApiClients.AzureTableClient;
using Zucchinimvc.Infrastructure.Config;
using Zucchinimvc.Infrastructure.Data;
using Zucchinimvc.Infrastructure.Repositories;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;
using Zucchinimvc.Services.Emails;
using Zucchinimvc.Services.Subscriptions;
using Zucchinimvc.Services.Users;
using Zucchinimvc.Infrastructure.Repositories.WeatherRepo;
using Zucchinimvc.Application.Services.Articles;


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
builder.Services.AddScoped<IHistoryRepository<WeatherHistoryEntity>>(sp => 
    {
        var provider = sp.GetRequiredService<IAzureTableClient>();
        var client = provider.GetClient("ExternalApiHistory");
        var logger = sp.GetRequiredService<ILogger<HistoryRepository<WeatherHistoryEntity>>>();
        
        return new HistoryRepository<WeatherHistoryEntity>(client, logger);
    });

// --- Services ---
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<ICmsService, CmsService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IUserService, UserService>();

// Email Services
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IEmailSender<User>, EmailSender>();
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSender>();

// Logger
builder.Services.AddScoped<IApiLoggerService, ApiLoggerService>();

builder.Services.Configure<CmsSettings>(
    builder.Configuration.GetSection("StrapiSettings"));

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<Roles>>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        await DbInitializer.SeedRoles(roleManager);
        await DbInitializer.SeedAdminAsync(userManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database seeding.");
    }
}

app.Run();
