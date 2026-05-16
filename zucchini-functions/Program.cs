using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Resend;
using zucchini_functions.Clients.QueueClients;
using zucchini_functions.Clients.ZucchiniApiClient;
using zucchini_functions.WeeklyNewsLetterEmail;
using Zucchinimvc.Infrastructure.Config;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddTransient<IWeeklyNewsLetter>(sp =>
{
    var options = sp.GetRequiredService<IOptions<WeeklyNewsLetterQueueSettings>>();
    var settings = options.Value;
    var queueClient = new ZucchiniQueueClient(settings.ConnectionString, settings.QueueName);
    return new WeeklyNewsLetter(queueClient);
});



builder.Services.AddHttpClient();
builder.Services.AddOptions<ResendClientOptions>()
    .Configure<IConfiguration>((options, configuration) =>
    {
        options.ApiToken = configuration["Resend:ApiKey"]!;
    });
builder.Services.AddTransient<IResend, ResendClient>();


builder.Services.AddHttpClient<IInternalUserClient, InternalUserClient>(
    client =>
    {
        client.BaseAddress =
            new Uri(builder.Configuration["ZucchiniInternal:ZucchiniBaseAddress"]!);
    });



builder.Build().Run();