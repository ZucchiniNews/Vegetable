namespace Zucchinimvc.Application.Services.Logger;

public interface IApiLoggerService
{
    void LogApiWarning(string apiName, string message);
    void LogApiError(string apiName, Exception ex);
}
