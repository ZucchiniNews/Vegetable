using Domain.Entities;

namespace Application.Interfaces;

public interface IWeatherRepository
{
    Task<GeoLocation?> GetCoordinatesAsync(string city);
    Task<WeatherSnapshot?> GetWeatherAsync(double lat, double lon);
}
