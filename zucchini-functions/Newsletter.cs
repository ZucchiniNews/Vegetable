using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace zucchini_functions;

public class Newsletter
{
    private readonly ILogger<Newsletter> _logger;

    public Newsletter(ILogger<Newsletter> logger)
    {
        _logger = logger;
    }

    [Function(nameof(Newsletter))]
    public void Run([QueueTrigger("newsletterqueue", Connection = "DefaultEndpointsProtocol=https;AccountName=zucchinibag;AccountKey=xwYp4c5ZhlGo+G+m29aHrgjmIYLhhDrKIrMKPZJaGcRTPM6ZZXoiFBdeco:vzhfANY4YzMtAOfMj+AStpXm1IA==;EndpointSuffix=core.windows.net")] QueueMessage message)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
    }
}