using Infrastrcture.Repositories.SubscriptionRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Articles;
using Zucchinimvc.Application.Services.CMS;
using Zucchinimvc.Application.Services.Emails;
using Zucchinimvc.Application.Services.Logger;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Application.Services.Users;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Infrastructure.ApiClients.AzureTableClient;
using Zucchinimvc.Infrastructure.ApiClients.SubscriptionPaymentClients;
using Zucchinimvc.Infrastructure.ApiClients.WeatherClient;
using Zucchinimvc.Infrastructure.Config;
using Zucchinimvc.Infrastructure.Data;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;
using Zucchinimvc.Infrastructure.Repositories.HistoryRepository;
using Zucchinimvc.Infrastructure.Repositories.IHistoryRepository;
using Zucchinimvc.Infrastructure.Repositories.SubscriptionRepo;
using Zucchinimvc.Infrastructure.Repositories.WeatherRepo;

var builder = WebApplication.CreateBuilder(args);

// Logger
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

// Configurations
builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherApi"));
builder.Services.Configure<CmsSettings>(builder.Configuration.GetSection("StrapiSettings"));
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

// Http Clients (Typed)
builder.Services.AddHttpClient<WeatherClient>();
builder.Services.AddHttpClient<CmsClient>();
builder.Services.AddSingleton<IAzureTableClient, AzureTableClient>();
builder.Services.AddScoped<CheckoutStripeClient>();

// Repositories
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ICmsRepository, CmsRepository>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IHistoryRepository<WeatherHistoryEntity>>(sp =>
{
    var provider = sp.GetRequiredService<IAzureTableClient>();
    var client = provider.GetClient("ExternalApiHistory");
    var logger = sp.GetRequiredService<ILogger<HistoryRepository<WeatherHistoryEntity>>>();
    // Explicit cast to the interface to resolve CS0266. 
    // If this still fails, ensure the HistoryRepository<T> type actually implements the exact IHistoryRepository<T>
    // (i.e. same namespace/assembly) you're referencing here.
    return (IHistoryRepository<WeatherHistoryEntity>)new HistoryRepository<WeatherHistoryEntity>(client, logger);
});

// Services
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<ICmsService, CmsService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IUserService, UserService>();

// Email Services
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IEmailSender<User>, EmailSender>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Logger
builder.Services.AddScoped<IApiLoggerService, ApiLoggerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        await DbInitializer.SeedRoles(roleManager);
        await DbInitializer.SeedAdminAsync(userManager);
        await DbInitializer.SeedPlansAsync(dbContext);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database seeding.");
    }
}

app.Run();
