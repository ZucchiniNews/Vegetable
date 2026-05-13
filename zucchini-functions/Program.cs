using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resend;
using zucchini_functions.Config;
using Zucchinimvc.Infrastructure.ApiClients.QueuePublisher;


var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.Services.Configure<NewsLetterSettings>(builder.Configuration.GetSection("NewsLetterSettings"));
builder.Services.AddTransient<AzureStorageQueue>();

// Register QueueClient for AzureStorageQueue
builder.Services.AddTransient(sp =>
{
    var connectionString = builder.Configuration["AzureWebJobsStorage"];
    var queueName = "newsletterqueue";
    return new QueueClient(connectionString, queueName);
});



// Register Resnd 
builder.Services.AddHttpClient<ResendClient>();
builder.Services.AddOptions<ResendClientOptions>()
    .Configure<IConfiguration>((options, configuration) =>
    {
        options.ApiToken =
            configuration["NewsLetterSettings:ApiToken"]!;
    });
builder.Services.AddTransient<IResend, ResendClient>();

builder.Build().Run();
