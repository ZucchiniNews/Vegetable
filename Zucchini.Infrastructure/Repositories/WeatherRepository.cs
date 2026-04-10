using Domain.Entities;
using Infrastructure.ApiClients.WeatherClient;
using Application.Services.Logger;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class WeatherRepository : RepositoryBase<WeatherRepository>, IWeatherRepository
{
    private readonly WeatherClient _client;

    public WeatherRepository(WeatherClient client, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _client = client;
    }

    public async Task<GeoLocation?> GetCoordinatesAsync(string city)
    {
        try
        {
            var result = await _client.GetAsync<List<GeoLocation>>(
                $"geo/1.0/direct?q={city}&limit=1");
            return result?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            logger.LogError("Geocoding", ex);
            return null;
        }
    }

    public async Task<WeatherResponse?> GetWeatherAsync(double lat, double lon)
    {
        try
        {
            return await _client.GetAsync<WeatherResponse>(
                $"data/2.5/weather?lat={lat}&lon={lon}&units=metric");
        }
        catch (Exception ex)
        {
            _apiLogger.LogApiError("WeatherAPI", ex);
            return null;
        }
    }
}