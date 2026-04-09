namespace Zucchini.Presentation.Models.DTOs.WeatherDTOs;
public class WeatherInfo
{
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;   // <img src="https://openweathermap.org/img/wn/@Model.Weather[0].Icon@2x.png" alt="Weather icon" />
}
