namespace Zucchinimvc.Models.ViewModels
{
    public class WeatherViewModel
    {
        public string City { get; set; } = "Linköping";
        public double Temp { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}
