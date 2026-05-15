using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resend;
using Zucchinimvc.Application.Services.Logger;
using Zucchinimvc.Application.Services.UsersService;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Infrastructure.ApiClients.AzureTableClient;
using Zucchinimvc.Infrastructure.ApiClients.QueuePublisher;
using Zucchinimvc.Infrastructure.ApiClients.WeatherClient;
using Zucchinimvc.Infrastructure.Config;
using Zucchinimvc.Infrastructure.Data;
using Zucchinimvc.Infrastructure.Repositories.HistoryRepo;
using Zucchinimvc.Infrastructure.Repositories.WeatherRepo;

var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.Services.AddTransient<AzureStorageQueue>();


builder.Services.AddTransient(sp =>
{
    var connectionString =
        builder.Configuration["AzureWebJobsStorage"];
    var QueueName = builder.Configuration["QueueName"]!;
    return new QueueClient(connectionString, QueueName);
});
builder.Services.AddHttpClient();
builder.Services.AddOptions<ResendClientOptions>()
    .Configure<IConfiguration>((options, configuration) =>
    {
        options.ApiToken =
            configuration["ApiToken"]!;
    });
builder.Services.AddTransient<IResend, ResendClient>();
builder.Services.AddScoped<IUserService, UserService>();



// Weather Related services and repositories would be registered here, similar to the previous example
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherApi"));
builder.Services.AddHttpClient<WeatherClient>();
builder.Services.AddSingleton<IAzureTableClient, AzureTableClient>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IApiLoggerService, ApiLoggerService>();
builder.Services.AddScoped(typeof(IHistoryRepository<>), typeof(HistoryRepository<>));
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();





builder.Build().Run();