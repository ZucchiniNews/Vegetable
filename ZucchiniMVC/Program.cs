using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLib.Clients.QueuePublisherClient;
using SharedLib.QueuePublishier;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Analytics;
using Zucchinimvc.Application.Services.Billing;
using Zucchinimvc.Application.Services.CMS;
using Zucchinimvc.Application.Services.Currency;
using Zucchinimvc.Application.Services.Emails;
using Zucchinimvc.Application.Services.Logger;
using Zucchinimvc.Application.Services.Plans;
using Zucchinimvc.Application.Services.Searches;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Application.Services.UsersService;
using Zucchinimvc.Application.Services.Utils;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Infrastructure.ApiClients.AzureInsightClient;
using Zucchinimvc.Infrastructure.ApiClients.AzureTableClient;
using Zucchinimvc.Infrastructure.ApiClients.CurrencyClient;
using Zucchinimvc.Infrastructure.ApiClients.LogQueryClient;
using Zucchinimvc.Infrastructure.ApiClients.WeatherClient;
using Zucchinimvc.Infrastructure.ApiClients.ZucchininSearchClient;
using Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient;
using Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient.Payments;
using Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient.Subscription;
using Zucchinimvc.Infrastructure.ApiFilter;
using Zucchinimvc.Infrastructure.Config;
using Zucchinimvc.Infrastructure.Data;
using Zucchinimvc.Infrastructure.Repositories.AnalyticsRepo;
using Zucchinimvc.Infrastructure.Repositories.BillingRepo;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;
using Zucchinimvc.Infrastructure.Repositories.CurrencyRepo;
using Zucchinimvc.Infrastructure.Repositories.HistoryRepo;
using Zucchinimvc.Infrastructure.Repositories.PlanRepo;
using Zucchinimvc.Infrastructure.Repositories.SearchRepo;
using Zucchinimvc.Infrastructure.Repositories.SubscriptionRepo;
using Zucchinimvc.Infrastructure.Repositories.WeatherRepo;
using ZucchiniMVC.Application.Services.Recommendation;

var builder = WebApplication.CreateBuilder(args);

// Logger
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Framework services
builder.Services.AddMemoryCache();

// Register filter
builder.Services.AddScoped<LayoutDataFilter>();

// MVC
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<LayoutDataFilter>();
});

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
builder.Services.Configure<CurrencySettings>(builder.Configuration.GetSection("CurrencyApi"));
builder.Services.Configure<SearchSettings>(builder.Configuration.GetSection("SearchSettings"));
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection("WelcomeEmailQueueSettings"));
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection("WeeklyNewsLetterQueueSettings"));

// Http Clients (Typed)
builder.Services.AddHttpClient<CmsClient>();
builder.Services.AddScoped<ZucchiniStripeClient>();
builder.Services.AddScoped<IProviderSubscription, ProviderSubscription>();
builder.Services.AddScoped<IProviderPayment, ProviderPayment>();
builder.Services.AddScoped<ZucchiniSearchClient>();

builder.Services.AddHttpClient<WeatherClient>();
builder.Services.AddHttpClient<CurrencyClient>();
builder.Services.AddScoped<IAzureTableClient, AzureTableClient>();
builder.Services.AddScoped<IAzureInsightClient, AzureInsightClient>();
builder.Services.AddScoped<ZuccLogQueryClient>();

builder.Services.AddTransient<IQueuePublisher>(sp =>
{
    var options = sp.GetRequiredService<IOptions<QueueSettings>>();
    var settings = options.Value;
    var queueClient = new ZucchiniQueueClient(settings.ConnectionString, settings.QueueName);
    return new ZucchiniQueuePublisher(queueClient);
});



// Services
builder.Services.AddScoped<IUtilsService, UtilsService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<ICmsService, CmsService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IEmailSender<User>, EmailSender>();
builder.Services.AddTransient<IEmailSender, EmailSender>();




// Repositories
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ICmsRepository, CmsRepository>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IBillingRepository, BillingRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddScoped<IApiLoggerService, ApiLoggerService>();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddScoped(typeof(IHistoryRepository<>), typeof(HistoryRepository<>));
// Logger

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles(); 
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

        await dbContext.Database.MigrateAsync();

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
