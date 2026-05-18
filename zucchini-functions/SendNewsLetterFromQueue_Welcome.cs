using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using zucchini_functions.NewsLetter;

namespace zucchini_functions;

public class SendNewsLetterFromQueue_Welcome
{
    private readonly ILogger<SendNewsLetterFromQueue_Welcome> _logger;
    private readonly INewsLetter _newsLetter;

    public SendNewsLetterFromQueue_Welcome(
        ILogger<SendNewsLetterFromQueue_Welcome> logger,
        INewsLetter newsLetter
        )
    {
        _logger = logger;
        _newsLetter = newsLetter;
    }

    [Function(nameof(SendNewsLetterFromQueue_Welcome))]
    public async Task RunWelcome(
        [QueueTrigger("wellcome-newsletterqueue", Connection = "ConnectionString")]
    string message)
    {
        _logger.LogInformation("Processing welcome queue message: {message}", message);
        await _newsLetter.SendEmail(message, CancellationToken.None);
    }
}


