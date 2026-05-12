using Application.Services.NewsLetter;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resend;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Logger;
using Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Infrastructure.ApiClients.AzureTableClient;
using Zucchinimvc.Infrastructure.ApiClients.QueuePublisher;
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
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection("QueueSettings"));

// 4. Clients
builder.Services.AddHttpClient<WeatherClient>();
builder.Services.AddSingleton(sp =>
{
    var queueSettings =
        sp.GetRequiredService<IOptions<QueueSettings>>().Value;

    var client = new QueueClient(
        queueSettings.ConnectionString,
        "newsletterqueue");

    client.CreateIfNotExists();

    return client;
});

builder.Services.AddSingleton<AzureStorageQueue>();

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

// 6. Newsletter auth client
builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(options =>
{
    options.ApiToken = builder.Configuration["NewsLetterEmailSettings:ApiKey"]!;
});
builder.Services.AddTransient<IResend, ResendClient>();

// 7. Services
builder.Services.AddTransient<INewsLetterQueuePublisher, AzureStorageQueueNewLetterPublisher>();
builder.Services.AddTransient<INewsLetterService, NewsLetterService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IApiLoggerService, ApiLoggerService>();

builder.Build().Run();