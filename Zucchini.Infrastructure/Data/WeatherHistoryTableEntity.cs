using Azure;
using Azure.Data.Tables;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;
public class WeatherHistoryTableEntity : IHistoryRecord, ITableEntity
{
    public string PartitionKey { get; set; } = "Linköping";
    public string RowKey { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd-HH");
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public string Condition { get; set; } = string.Empty;

    public static WeatherHistoryTableEntity FromDomain(WeatherHistory entity) => new()
    {
        Temperature = entity.Temperature,
        Humidity = entity.Humidity,
        Condition = entity.Condition
    };

    public WeatherHistory ToDomain() => new()
    {
        RecordedAt = Timestamp?.UtcDateTime ?? DateTime.UtcNow,
        Temperature = Temperature,
        Humidity = Humidity,
        Condition = Condition
    };
}
