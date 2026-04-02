namespace Zucchinimvc.Infrastructure.Config
{
    public class OpenWeatherSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.openweathermap.org/";
    }
}
