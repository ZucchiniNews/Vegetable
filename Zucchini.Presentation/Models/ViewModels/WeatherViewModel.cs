namespace Zucchinimvc.Models.ViewModels
{
    public class WeatherViewModel
    {
        public string City { get; set; } = string.Empty;
        public double Temp { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int Humidity { get; set; } = 0;
        public string IconUrl => $"https://openweathermap.org/img/wn/{Icon}@2x.png";

        public List<CityWeatherChart> Cities { get; set; } = new();

        // for the history, chartJS
        public List<string> Labels { get; set; } = new();
        public List<double> Temperatures { get; set; } = new();


    }
}
