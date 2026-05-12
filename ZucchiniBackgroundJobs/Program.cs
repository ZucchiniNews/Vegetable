using Application.Services.NewsLetter;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Logger;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Infrastructure.ApiClients.AzureTableClient;
using Zucchinimvc.Infrastructure.ApiClients.NewsLetterEmailClient;
using Zucchinimvc.Infrastructure.ApiClients.WeatherClient;
using Zucchinimvc.Infrastructure.Config;
using Zucchinimvc.Infrastructure.Data;
using Zucchinimvc.Infrastructure.Repositories.HistoryRepo;
using Zucchinimvc.Infrastructure.Repositories.WeatherRepo;
using ZucchiniMVC.Application.Services.NewsLetter;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// 1. Application Insights & Telemetry
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// 2. Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Configuration
builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherApi"));
builder.Services.Configure<NewsLetterSettings>(builder.Configuration.GetSection("NewsLetterSettings"));

// 4. Clients
builder.Services.AddHttpClient<WeatherClient>();
builder.Services.AddHttpClient<NewsLetterEmailClient>();

// 5. Repositories
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddSingleton<IAzureTableClient, AzureTableClient>();
builder.Services.AddScoped<IHistoryRepository<WeatherHistoryEntity>>(sp =>
{
    var provider = sp.GetRequiredService<IAzureTableClient>();
    var client = provider.GetClient("ExternalApiHistory");
    var logger = sp.GetRequiredService<ILogger<HistoryRepository<WeatherHistoryEntity>>>();
    return new HistoryRepository<WeatherHistoryEntity>(client, logger);
});

// 6. Services
builder.Services.AddTransient<INewsLetterService, NewsLetterService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IApiLoggerService, ApiLoggerService>();

builder.Build().Run();