using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    public SendNewsLetter(ILogger<SendNewsLetter> logger, IResend resend, IOptions<NewsLetterSettings> settings)
    {
        _logger = logger;
        _resend = resend;
        _settings = settings.Value;
    }

    [Function(nameof(SendNewsLetter))]
    public async Task Run(
    [QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")]
    string message)
    {
        try
        {
            _logger.LogInformation("Processing queue message: {message}", message);


            var newsletterMessage =
                JsonSerializer.Deserialize<NewsLetterQueueMessage>(
                    message,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (newsletterMessage is null)
                throw new InvalidOperationException("Invalid queue payload");

            _logger.LogInformation("FROM EMAIL: {from}", _settings.FromEmail);
            _logger.LogInformation("TO EMAIL: {to}", newsletterMessage.Email);

            var emailMessage = new EmailMessage
            {
                From = _settings.FromEmail,
                To = newsletterMessage.Email,
                Subject = newsletterMessage.Subject,
                HtmlBody = newsletterMessage.HtmlBody
            };

            await _resend.EmailSendAsync(emailMessage);

            _logger.LogInformation("Email sent to {email}", newsletterMessage.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Newsletter function failed");
            throw;
        }
    }
}


