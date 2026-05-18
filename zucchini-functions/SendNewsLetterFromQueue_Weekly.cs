using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using zucchini_functions.NewsLetter;

namespace zucchini_functions;

public class SendNewsLetterFromQueue_Weekly
{
    private readonly ILogger<SendNewsLetterFromQueue_Weekly> _logger;
    private readonly INewsLetter _newsLetter;

    public SendNewsLetterFromQueue_Weekly(
        ILogger<SendNewsLetterFromQueue_Weekly> logger,
        INewsLetter newsLetter
        )
    {
        _logger = logger;
        _newsLetter = newsLetter;
    }

    [Function(nameof(SendNewsLetterFromQueue_Weekly))]
    public async Task RunWeekly(
       [QueueTrigger("weekly-sending-newsletter", Connection = "ConnectionString")]
    string message)
    {
        _logger.LogInformation("Processing weekly queue message: {message}", message);
        await _newsLetter.SendEmail(message, CancellationToken.None);
    }
}