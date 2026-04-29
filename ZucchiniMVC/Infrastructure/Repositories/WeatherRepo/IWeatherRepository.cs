using ZucchiniCore.Entities;
using Zucchinimvc.Models.DTOs.WeatherDTOs;

namespace Zucchinimvc.Infrastructure.Repositories.WeatherRepo
{
    public interface IWeatherRepository
    {
        Task<GeoLocation?> GetCoordinatesAsync(string city);
        Task<WeatherResponse?> GetWeatherAsync(double lat, double lon);
    }

}

