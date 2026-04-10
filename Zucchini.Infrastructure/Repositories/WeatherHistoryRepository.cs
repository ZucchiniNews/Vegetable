using Application.Interfaces;
using Azure.Data.Tables;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class WeatherHistoryRepository
    : HistoryRepository<WeatherHistoryTableEntity>,
      IHistoryRepository<WeatherHistory>
{
    public WeatherHistoryRepository(TableClient client, ILoggerFactory lf)
        : base(client, lf) { }

    public async Task UpsertAsync(WeatherHistory entity)
        => await base.UpsertAsync(WeatherHistoryTableEntity.FromDomain(entity));

    public async Task<IEnumerable<WeatherHistory>> GetAllAsync()
        => (await base.GetAllAsync()).Select(e => e.ToDomain());

    public async Task<IEnumerable<WeatherHistory>> GetDailyHistoryAsync(string city, int days)
    {
        var entities = await base.GetDailyHistoryAsync(city, days);
        return entities.Select(e => e.ToDomain());
    }

    public async Task<IEnumerable<WeatherHistory>> GetRecentByPartitionKeyAsync(string partitionKey, int take = 50)
    {
        var entities = await base.GetRecentByPartitionKeyAsync(partitionKey, take);
        return entities.Select(e => e.ToDomain());
    }
}