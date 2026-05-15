using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue;
using Zucchinimvc.Application.Services.UsersService;

public class WeeklyNewsLetterQueuePublisher
{
    private readonly ILogger _logger;
    private readonly AzureStorageQueueNewLetterPublisher _azureStorageQueueNewLetterPublisher;
    private readonly IUserService _userService;


    public WeeklyNewsLetterQueuePublisher(
        ILoggerFactory loggerFactory,
        AzureStorageQueueNewLetterPublisher azureStorageQueueNewLetterPublisher,
        IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<WeeklyNewsLetterQueuePublisher>();
        _azureStorageQueueNewLetterPublisher = azureStorageQueueNewLetterPublisher;
        _userService = userService;
    }

    [Function("SendWeeklyNewsLetter")]
    public async Task Run(
        [TimerTrigger("0 0 18 * * 5")] TimerInfo myTimer)
    {
        _logger.LogInformation(
            "Weekly newsletter started at: {time}",
            DateTime.UtcNow);

        var subscribers = await _userService.GetNewsletterSubscribersAsync();

        foreach (var user in subscribers)
        {
            var message = new NewsLetterQueueMessage
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