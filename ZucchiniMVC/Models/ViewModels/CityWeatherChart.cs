namespace Zucchinimvc.Models.ViewModels
{
    public class CityWeatherChart
    {
        public string City { get; set; } = "City";
        public List<DateTime> Labels { get; set; } = new List<DateTime>();
        public List<double> Temperatures { get; set; } = new List<double>();
    }

}
