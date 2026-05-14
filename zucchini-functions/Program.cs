using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resend;
using Zucchinimvc.Infrastructure.ApiClients.QueuePublisher;

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

builder.Build().Run();