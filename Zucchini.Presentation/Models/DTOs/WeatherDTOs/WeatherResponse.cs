namespace Zucchinimvc.Models.DTOs.WeatherDTOs;

public class WeatherResponse
{
    public MainInfo? Main { get; set; }
    public List<WeatherInfo>? Weather { get; set; }
    public string Name { get; set; } = string.Empty;
}




