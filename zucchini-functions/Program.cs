using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Resend;
using SharedLib.Clients.conifgs;
using SharedLib.Clients.QueuePublisherClient;
using zucchini_functions.Clients.ZucchiniApiClient;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.Configure<NewsLetterQueueSettings>(
    builder.Configuration.GetSection("WeeklyNewsLetterQueue"));

builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IOptions<NewsLetterQueueSettings>>().Value;

    return new ZucchiniQueueClient(
        settings.ConnectionString,
        settings.QueueName);
});

// Resend 
builder.Services.Configure<ResendClientOptions>(
    builder.Configuration.GetSection("Resend"));

builder.Services.AddSingleton<IResend, ResendClient>();

//  Internal Zucchini API Client
builder.Services.AddHttpClient<IInternalUserClient, InternalUserClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ZucchiniInternal:BaseUrl"]!);
});





builder.Build().Run();