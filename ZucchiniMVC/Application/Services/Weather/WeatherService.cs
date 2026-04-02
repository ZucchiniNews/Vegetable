using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.ApiClients.OpenWeatherClient;
using Zucchinimvc.Infrastructure.Repositories;
using Zucchinimvc.Models.DTOs.WeatherDTOs;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Application.Services.Weather;

public class WeatherService : IWeatherService
{
    private readonly IHistoryRepository<WeatherHistoryEntity> _repository;
    private readonly WeatherClient _client;

    public WeatherService(WeatherClient client, IHistoryRepository<WeatherHistoryEntity> repository)
    {
        _client = client;
        _repository = repository;
    }

    public async Task<WeatherViewModel?> GetWeatherByCityAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city) || !_client.IsConfigured)
            return null;

        // 1. Use the client to get coordinates
        var location = await GetCoordinatesAsync(city);
        if (location == null) return new WeatherViewModel { City = city };

        // 2. Use the client to get weather
        var weather = await GetWeatherAsync(location.Lat, location.Lon);
        if (weather?.Main == null) return null;

        return new WeatherViewModel
        {
            City = city,
            Temp = weather.Main.Temp,
            Humidity = weather.Main.Humidity,
            Description = weather.Weather?.FirstOrDefault()?.Description ?? "",
            Icon = weather.Weather?.FirstOrDefault()?.Icon ?? ""
        };
    }

    public async Task SaveWeatherHistoryAsync(WeatherViewModel model)
    {
        var entity = new WeatherHistoryEntity
        {
            PartitionKey = model.City,
            RowKey = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm"),
            Temperature = model.Temp,
            Humidity = model.Humidity,
            Condition = model.Description,
            RecordedAt = DateTime.UtcNow,
        };

        await _repository.UpsertDailyAsync(entity);
    }

    public async Task<GeoLocation?> GetCoordinatesAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city) || !_client.IsConfigured)
            return null;

        var results = await _client.GetAsync<List<GeoLocation>>(
            $"geo/1.0/direct?q={Uri.EscapeDataString(city)}&limit=1");

        return results?.FirstOrDefault();
    }

    public async Task<WeatherResponse?> GetWeatherAsync(double lat, double lon)
    {
        if (!_client.IsConfigured) return null;

        return await _client.GetAsync<WeatherResponse>(
            $"data/2.5/weather?lat={lat}&lon={lon}&units=metric");
    }

    public async Task<List<WeatherHistoryEntity>> GetWeatherAsync()
    {
        var data = await _repository.GetAllAsync();

        return data?
            .OrderBy(e => e.RecordedAt)
            .ToList() ?? new List<WeatherHistoryEntity>();
    }

    public async Task<List<WeatherHistoryEntity>> GetHistoryAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return new List<WeatherHistoryEntity>();

        var data = await _repository.GetRecentByPartitionKeyAsync(city, 10);

        return data?
            .OrderBy(x => x.RecordedAt)
            .ToList() ?? new List<WeatherHistoryEntity>();
    }
}