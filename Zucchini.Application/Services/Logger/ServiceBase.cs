using Microsoft.Extensions.Logging;

namespace Application.Services.Logger;
public abstract class ServiceBase<T>
{
    protected readonly ILogger logger;

    protected ServiceBase(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<T>();
    }
}