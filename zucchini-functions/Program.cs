using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Resend;
using Zucchinimvc.Application.Services.QueuePublishie.WelcomeToNewsLetterEmail;
using Zucchinimvc.Application.Services.QueuePublishier.WeeklyNewsLetterEmail;
using Zucchinimvc.Application.Services.UsersService;
using Zucchinimvc.Infrastructure.ApiClients.QueuePublisher;
using Zucchinimvc.Infrastructure.Config;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services.AddTransient<IWeeklyNewsLetterPublisher>(sp =>
{
    var options = sp.GetRequiredService<IOptions<WeeklyNewsLetterQueueSettings>>();
    var settings = options.Value;
    var queueClient = new ZucchiniQueueClient(settings.ConnectionString, settings.QueueName);
    return new WeeklyNewsLetterPublisher(queueClient);
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