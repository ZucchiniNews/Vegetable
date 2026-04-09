using Microsoft.Extensions.Logging;

namespace Zucchinimvc.Application.Services.Logger;

public class ApiLoggerService : IApiLoggerService
{
    private readonly ILogger<ApiLoggerService> _logger;

    public ApiLoggerService(ILogger<ApiLoggerService> logger)
    {
        _logger = logger;
    }

    public void LogApiWarning(string apiName, string message)
    {
        _logger.LogWarning("[API: {Api}] {Message}", apiName, message);
    }

    public void LogApiError(string apiName, Exception ex)
    {
        _logger.LogError(ex, "[API: {Api}] An error occurred", apiName);
    }
}
