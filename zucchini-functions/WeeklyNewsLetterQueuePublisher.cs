using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SharedLib.DTOs.NewsLetterSubscriber;
using SharedLib.DTOs.QueuePublisherDOTs;
using SharedLib.QueuePublisher;
using zucchini_functions.Clients.ZucchiniApiClient;


public class WeeklyNewsLetterQueuePublisher
{
    private readonly ILogger _logger;
    private readonly IQueuePublisher _queuePublisher;
    private readonly IZucchiniClient _zucchiniClient;

    public WeeklyNewsLetterQueuePublisher(
        ILoggerFactory loggerFactory,
        IQueuePublisher queuePublisher,
        IZucchiniClient zucchiniClient
        )
    {
        _logger = loggerFactory.CreateLogger<WeeklyNewsLetterQueuePublisher>();
        _queuePublisher = queuePublisher;
        _zucchiniClient = zucchiniClient;
    }

    [Function("WeeklyNewsLetterQueuePublisher")]
    public async Task Run(
        [TimerTrigger("0 0 18 * * 5")] TimerInfo myTimer)
    {
        _logger.LogInformation(
            "Weekly newsletter started at: {time}",
            DateTime.UtcNow);

        List<NewsletterSubscriberDto> subscribers = await _zucchiniClient.GetSubscribedUsersAsync();

        foreach (NewsletterSubscriberDto user in subscribers)
        {
            var message = new NewsLetterQueueDto
            {
                Email = user.Email!,
                Subject = "Weekly Newsletter",
                HtmlBody = "<h1>Hello!</h1>"
            };

            await _queuePublisher.PublishAsync(message, CancellationToken.None);

            _logger.LogInformation(
                "Queued weekly newsletter for {email}",
                user.Email);
        }
    }
}