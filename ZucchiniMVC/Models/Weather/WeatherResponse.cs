namespace Zucchinimvc.Models.Weather
{
    public class WeatherResponse
    {
        public MainInfo? Main { get; set; }
        public List<WeatherInfo>? Weather { get; set; }

        public string Name { get; set; } = string.Empty; // City name
    }

    public class MainInfo
    {
        public double Temp { get; set; }
        public double Feels_like { get; set; }
        public double Temp_min { get; set; }
        public double Temp_max { get; set; }
        public int Humidity { get; set; }
    }

    public class WeatherInfo
    {
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;   // <img src="https://openweathermap.org/img/wn/@Model.Weather[0].Icon@2x.png" alt="Weather icon" />
    }

}
