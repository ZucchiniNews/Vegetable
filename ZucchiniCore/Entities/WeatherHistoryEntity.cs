using Azure;
using Azure.Data.Tables;

namespace ZucchiniCore.Entities
{
    public class WeatherHistoryEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "Linköping";
        public string RowKey { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd-HH");

        public DateTimeOffset? Timestamp { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
        public ETag ETag { get; set; }

        // weather data
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public string Condition { get; set; } = string.Empty;
    }
}
