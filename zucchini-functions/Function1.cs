using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace zucchini_functions;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    [Function(nameof(Function1))]
    public void Run([QueueTrigger("newsletterqueue", Connection = "DefaultEndpointsProtocol=https;AccountName=zucchinibag;AccountKey=xwYp4c5ZhlGo+G+m29aHrgjmIYLhhDrKIrMKPZJaGcRTPM6ZZXoiFBdeco:vzhfANY4YzMtAOfMj+AStpXm1IA==;EndpointSuffix=core.windows.net")] QueueMessage message)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
    }
}