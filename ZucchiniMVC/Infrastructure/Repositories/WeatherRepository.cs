using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.ApiClients.WeatherClient;
using Zucchinimvc.Models.DTOs.WeatherDTOs;
using Zucchinimvc.Application.Services.Logger;

namespace Zucchinimvc.Infrastructure.Repositories;

public class WeatherRepository : IWeatherRepository
{
    private readonly WeatherClient _client;
    private readonly IApiLoggerService _apiLogger;
    public WeatherRepository(WeatherClient client, IApiLoggerService apiLogger)
    {
        _client = client;
        _apiLogger = apiLogger;
    }

    public async Task<GeoLocation?> GetCoordinatesAsync(string city)
    {
        try
        {
            var result = await _client.GetAsync<List<GeoLocation>>(
                $"geo/1.0/direct?q={city}&limit=1");

            if (result == null)
                _apiLogger.LogApiWarning("Weather", "Geocoding failed for " + city);

            return result?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _apiLogger.LogApiError("Weather", ex);
            return null;
        }
    }

    public async Task<WeatherResponse?> GetWeatherAsync(double lat, double lon)
    {
        try
        {
            var result = await _client.GetAsync<WeatherResponse>(
                $"data/2.5/weather?lat={lat}&lon={lon}&units=metric");

            if (result == null)
                _apiLogger.LogApiWarning("Weather", $"Weather fetch failed for coords {lat},{lon}");

            return result;
        }
        catch (Exception ex)
        {
            _apiLogger.LogApiError("Weather", ex);
            return null;
        }
    }
}