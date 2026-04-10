using Domain.Interfaces;

namespace Domain.Entities;
// Represents a saved record for the predefined cities — lives in Azure Table
public class WeatherHistory : IHistoryRecord
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public string City { get; set; } = "Linköping";
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public string Condition { get; set; } = string.Empty;
    string IHistoryRecord.PartitionKey { get => PartitionKey; set => PartitionKey = value; }
    string IHistoryRecord.RowKey { get => RowKey; set => RowKey = value; }
}
