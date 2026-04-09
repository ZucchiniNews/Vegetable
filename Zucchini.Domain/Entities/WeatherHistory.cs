namespace Domain.Entities;
public class WeatherHistory
{
    public string City { get; set; } = "Linköping";
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public string Condition { get; set; } = string.Empty;
}
