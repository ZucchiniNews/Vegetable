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
using Zucchinimvc.Infrastructure.ApiClients.WeatherClient;
using Zucchinimvc.Infrastructure.Config;
using Zucchinimvc.Infrastructure.Data;
using Zucchinimvc.Infrastructure.Repositories.HistoryRepository;
using Zucchinimvc.Infrastructure.Repositories.IHistoryRepository;
using Zucchinimvc.Infrastructure.Repositories.WeatherRepo;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// 1. Application Insights & Telemetry
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// 2. Database Connection (If WeatherRepository uses DB)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Configurations (Settings for API Keys, etc.)
builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherApi"));

// 4. Clients and Repositories
builder.Services.AddHttpClient<WeatherClient>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

// 5. Services
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IApiLoggerService, ApiLoggerService>();

// 6. History Repository Registration
builder.Services.AddSingleton<IAzureTableClient, AzureTableClient>();

builder.Services.AddScoped<IHistoryRepository<WeatherHistoryEntity>>(sp =>
{
    var provider = sp.GetRequiredService<IAzureTableClient>();
    var client = provider.GetClient("ExternalApiHistory");
    var logger = sp.GetRequiredService<ILogger<HistoryRepository<WeatherHistoryEntity>>>();
    return new HistoryRepository<WeatherHistoryEntity>(client, logger);
});

builder.Build().Run();