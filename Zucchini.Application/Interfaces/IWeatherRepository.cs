using Domain.Entities;
// using Zucchini.Presentation.Models.DTOs.WeatherDTOs; // Shound NOT refer to Presentaion. need workaround

namespace Application.Interfaces;

public interface IWeatherRepository
{
    Task<GeoLocation?> GetCoordinatesAsync(string city);
    Task<WeatherResponse?> GetWeatherAsync(double lat, double lon);
}
