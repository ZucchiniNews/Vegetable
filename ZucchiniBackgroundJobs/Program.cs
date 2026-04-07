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
using Zucchinimvc.Infrastructure.ApiClients.WeatherClient;
using Zucchinimvc.Infrastructure.Config;
using Zucchinimvc.Infrastructure.Data;
using Zucchinimvc.Infrastructure.Repositories;

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

// 6. History Repository
builder.Services.AddScoped<IHistoryRepository<WeatherHistoryEntity>>(sp =>
    new HistoryRepository<WeatherHistoryEntity>(
        sp.GetRequiredService<IConfiguration>(),
        sp.GetRequiredService<ILogger<HistoryRepository<WeatherHistoryEntity>>>(),
        "ExternalApiHistory"
    ));

builder.Build().Run();