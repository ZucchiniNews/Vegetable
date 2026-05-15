using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resend;
using Zucchinimvc.Application.Services.QueuePublishie.WelcomeToNewsLetterEmail;
using Zucchinimvc.Application.Services.QueuePublishier.WeeklyNewsLetterEmail;
using Zucchinimvc.Application.Services.UsersService;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddTransient(sp =>
{
    var connectionString =
        builder.Configuration["AzureWebJobsStorage"];

    var queueName =
        builder.Configuration["QueueName"]!;

    return new QueueClient(connectionString, queueName);
});

builder.Services.AddHttpClient();

builder.Services.AddOptions<ResendClientOptions>()
    .Configure<IConfiguration>((options, configuration) =>
    {
        options.ApiToken = configuration["ApiToken"]!;
    });

builder.Services.AddTransient<IResend, ResendClient>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWeeklyNewsLetterPublisher, WeeklyNewsLetterPublisher>();

builder.Build().Run();