namespace Zucchinimvc.Models.ViewModels
{
    public class WeatherViewModel
    {
        public string City { get; set; } = string.Empty;
        public double Temp { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string IconUrl => $"https://openweathermap.org/img/wn/{Icon}@2x.png"; 
    }
}
