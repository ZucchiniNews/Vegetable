namespace Presentation.Models.ViewModels;
public class CityWeatherChart
{
    public string City { get; set; } = "City";
    public List<string> Labels { get; set; } = new List<string>();
    public List<double> Temperatures { get; set; } = new List<double>();
}
