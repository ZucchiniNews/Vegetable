using ZucchiniCore.Entities;
using Zucchinimvc.Models.DTOs.WeatherDTOs;

namespace Zucchinimvc.Infrastructure.Repositories;

public interface IWeatherRepository
{
    Task<WeatherResponse?> GetWeatherAsync(double lat, double lon);
}