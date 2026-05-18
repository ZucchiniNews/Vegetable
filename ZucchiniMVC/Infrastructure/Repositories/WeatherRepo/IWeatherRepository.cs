using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Weather.DTOs;

namespace Zucchinimvc.Infrastructure.Repositories.WeatherRepo
{
    public interface IWeatherRepository
    {
        Task<GeoLocation?> GetCoordinatesAsync(string city);
        Task<WeatherResponse?> GetWeatherAsync(double lat, double lon);
    }

}

