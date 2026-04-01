using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Infrastrcture.Repositories;


var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddHttpClient<WeatherService>();

builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.AddScoped<IHistoryRepository<WeatherHistoryEntity>>(sp =>
    new HistoryRepository<WeatherHistoryEntity>(
        sp.GetRequiredService<IConfiguration>(),
        sp.GetRequiredService<ILogger<HistoryRepository<WeatherHistoryEntity>>>(),
        "ExternalApiHistory"
    ));
builder.Build().Run();