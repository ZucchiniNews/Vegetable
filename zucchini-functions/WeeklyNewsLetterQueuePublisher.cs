using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using zucchini_functions.Clients.ZucchiniApiClient;
using zucchini_functions.WeeklyNewsLetterEmail;
using zucchini_functions.WeeklyNewsLetterEmail.DTOs;


public class WeeklyNewsLetterQueuePublisher
{
    private readonly ILogger _logger;
    private readonly WeeklyNewsLetter _azureStorageQueueNewLetterPublisher;
    private readonly IInternalUserClient _internalUserClient;

    public WeeklyNewsLetterQueuePublisher(
        ILoggerFactory loggerFactory,
        WeeklyNewsLetter azureStorageQueueNewLetterPublisher,
        IInternalUserClient internalUserClient
        )
    {
        _logger = loggerFactory.CreateLogger<WeeklyNewsLetterQueuePublisher>();
        _azureStorageQueueNewLetterPublisher = azureStorageQueueNewLetterPublisher;
        _internalUserClient = internalUserClient;
    }

    [Function("SendWeeklyNewsLetter")]
    public async Task Run(
        [TimerTrigger("0 0 18 * * 5")] TimerInfo myTimer)
    {
        _logger.LogInformation(
            "Weekly newsletter started at: {time}",
            DateTime.UtcNow);

        List<NewsletterSubscriberDto> subscribers = await _internalUserClient.GetSubscribedUsersAsync();

        foreach (NewsletterSubscriberDto user in subscribers)
        {
            var message = new NewsLetterQueueDto
            {
                Email = user.Email!,
                Subject = "Weekly Newsletter",
                HtmlBody = "<h1>Hello!</h1>"
            };

            var json = JsonSerializer.Serialize(message);

            await _azureStorageQueueNewLetterPublisher.PublishAsync(message, CancellationToken.None);

            _logger.LogInformation(
                "Queued weekly newsletter for {email}",
                user.Email);
        }
    }
}