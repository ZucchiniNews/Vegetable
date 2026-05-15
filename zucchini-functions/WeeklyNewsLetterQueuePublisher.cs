using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.UsersService;

public class WeeklyNewsLetterQueuePublisher
{
    private readonly ILogger _logger;
    private readonly QueueClient _queueClient;
    private readonly IUserService _userService;


    public WeeklyNewsLetterQueuePublisher(
        ILoggerFactory loggerFactory,
        QueueClient queueClient,
        IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<WeeklyNewsLetterQueuePublisher>();
        _queueClient = queueClient;
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

            await _queueClient.SendMessageAsync(json);

            _logger.LogInformation(
                "Queued newsletter for {email}",
                user.Email);
        }
    }
}