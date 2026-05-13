using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue;
using ZucchiniMVC.Application.Services.NewsLetter;


namespace zucchini_functions;

public class NewsletterWorkerFunction
{
    private readonly ILogger<NewsletterWorkerFunction> _logger;
    private readonly INewsLetterService _sender;

    public NewsletterWorkerFunction(ILogger<NewsletterWorkerFunction> logger, INewsLetterService sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [Function(nameof(NewsletterWorkerFunction))]
    public async Task Run([QueueTrigger("newsletterqueue", Connection = "DefaultEndpointsProtocol=https;AccountName=zucchinibag;AccountKey=xwYp4c5ZhlGo+G+m29aHrgjmIYLhhDrKIrMKPZJaGcRTPM6ZZXoiFBdeco:vzhfANY4YzMtAOfMj+AStpXm1IA==;EndpointSuffix=core.windows.net")] QueueMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText, cancellationToken);
        var newsletterMessage = JsonSerializer.Deserialize<NewsLetterQueueMessage>(message.MessageText);
        if (newsletterMessage is null)
        {
            _logger.LogError("Newsletter queue message payload was invalid.");
            throw new InvalidOperationException("Newsletter queue message payload was invalid.");
        }
        await _sender.SendNewsLetterEmailAsync(
             newsletterMessage.Email,
             newsletterMessage.Subject,
             newsletterMessage.HtmlBody,
             cancellationToken).ConfigureAwait(false);


    }
}


