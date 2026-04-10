namespace Domain.Entities;

// Represents a live API fetch result
public class WeatherSnapshot
{
    public string City { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}