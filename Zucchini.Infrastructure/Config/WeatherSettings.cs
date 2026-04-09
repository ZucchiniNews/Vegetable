namespace Infrastructure.Config;

public class WeatherSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.openweathermap.org/";
}
