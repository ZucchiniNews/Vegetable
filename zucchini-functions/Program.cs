using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Resend;
using SharedLib.Clients.QueuePublisherClient;
using SharedLib.QueuePublishier;
using zucchini_functions.Clients.ZucchiniApiClient;
using zucchini_functions.NewsLetter;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();



builder.Services.Configure<QueueSettings>(
    builder.Configuration.GetSection("NewsLetterQueueSettings"));

builder.Services.AddTransient<IQueuePublisher>(sp =>
{
    var options = sp.GetRequiredService<IOptions<QueueSettings>>();
    var settings = options.Value;
    var queueClient = new ZucchiniQueueClient(settings.ConnectionString, settings.QueueName);
    return new ZucchiniQueuePublisher(queueClient);
});

// Resend 
builder.Services.AddHttpClient();

builder.Services.AddOptions<ResendClientOptions>()
    .Configure<IConfiguration>((options, configuration) =>
    {
        options.ApiToken = configuration["ResendApiKey"]!;
    });

builder.Services.AddTransient<IResend, ResendClient>();

//  Internal Zucchini API Client
builder.Services.AddHttpClient<IZucchiniClient, ZucchiniClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ZucchiniInternal:BaseUrl"]!);
});

// Nesletter Service
builder.Services.AddTransient<INewsLetter, NewsLetter>();


builder.Build().Run();