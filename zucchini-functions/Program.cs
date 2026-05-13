using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zucchinimvc.Application.Services.NewsLetter;
using Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue;
using ZucchiniMVC.Application.Services.NewsLetter;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddTransient<INewsLetterQueuePublisher, AzureStorageQueueNewLetterPublisher>();
builder.Services.AddTransient<INewsLetterService, NewsLetterService>();

builder.Build().Run();
