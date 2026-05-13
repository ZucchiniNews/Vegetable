using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Resend;
using System.Text.Json;
using zucchini_functions.Config;
using Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue;


namespace zucchini_functions;

public class SendNewsLetter
{
    private readonly ILogger<SendNewsLetter> _logger;
    private readonly IResend _resend;
    private readonly NewsLetterSettings _settings;
    public SendNewsLetter(ILogger<SendNewsLetter> logger, IResend resend, NewsLetterSettings settings)
    {
        _logger = logger;
        _resend = resend;
        _settings = settings;
    }

    [Function(nameof(SendNewsLetter))]
    public async Task Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] QueueMessage message)
    {
        Console.WriteLine("C# Queue trigger function processed: {0}", message);
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
        var newsletterMessage = JsonSerializer.Deserialize<NewsLetterQueueMessage>(message.MessageText);
        if (newsletterMessage is null)
        {
            _logger.LogError("Newsletter queue message payload was invalid.");
            throw new InvalidOperationException("Newsletter queue message payload was invalid.");
        }

        var emailMessage = new EmailMessage
        {
            From = _settings.FromEmail,
            To = newsletterMessage.Email,
            Subject = newsletterMessage.Subject,
            HtmlBody = newsletterMessage.HtmlBody
        };

        await _resend.EmailSendAsync(emailMessage).ConfigureAwait(false);
    }
}


