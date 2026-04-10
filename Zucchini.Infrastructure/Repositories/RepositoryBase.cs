
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public abstract class RepositoryBase<T>
{
    protected readonly ILogger logger;

    public RepositoryBase(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<T>();
    }
}