using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Resend;
using System.Text.Json;
using zucchini_functions.WeeklyNewsLetterEmail.DTOs;

namespace zucchini_functions;

public class SendWeeklyNewsLetter
{
    private readonly ILogger<SendWeeklyNewsLetter> _logger;
    private readonly IResend _resend;
    public SendWeeklyNewsLetter(ILogger<SendWeeklyNewsLetter> logger, IResend resend)
    {
        _logger = logger;
        _resend = resend;
    }

    [Function(nameof(SendWeeklyNewsLetter))]
    public async Task Run(
    [QueueTrigger("weekly-sending-newsletter", Connection = "AzureWebJobsStorage")]
    string message)
    {
        try
        {
            _logger.LogInformation("Processing queue message: {message}", message);
            var EmailFrom = Environment.GetEnvironmentVariable("Resend:FROM_EMAIL") ?? throw new InvalidOperationException("FROM_EMAIL environment variable is not set");

            var newsletterMessage =
                JsonSerializer.Deserialize<NewsLetterQueueDto>(
                    message,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (newsletterMessage is null)
                throw new InvalidOperationException("Invalid queue payload");


            _logger.LogInformation("TO EMAIL: {to}", newsletterMessage.Email);

            var emailMessage = new EmailMessage
            {
                From = EmailFrom,
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


