using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ZucchiniBackgroundJobs.Functions;

public class NewsLetter
{
    private readonly ILogger<NewsLetter> _logger;

    public NewsLetter(ILogger<NewsLetter> logger)
    {
        _logger = logger;
    }

    [Function("NewsLetter")]
    public async Task Run([QueueTrigger("newsletterqueue", Connection = "AzureStorage")] QueueMessage message)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
        Console.WriteLine("hello the function is triggered locally");
        await Task.CompletedTask;
    }
}