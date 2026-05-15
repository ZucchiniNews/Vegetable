using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zucchinimvc.Application.Services.Logger;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Infrastructure.ApiClients.AzureTableClient;
using Zucchinimvc.Infrastructure.ApiClients.WeatherClient;
using Zucchinimvc.Infrastructure.Config;
using Zucchinimvc.Infrastructure.Data;
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
builder.Services.AddSingleton<IAzureTableClient, AzureTableClient>();




// 5. Services
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IApiLoggerService, ApiLoggerService>();

// 6. Repository Registration


builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

builder.Build().Run();