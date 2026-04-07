using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Repositories;
using Zucchinimvc.Models.DTOs.WeatherDTOs;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Application.Services.Weather;

public class WeatherService : IWeatherService
{
    private readonly IWeatherRepository _weatherRepo;
    private readonly IApiLoggerService _apiLogger;

    public WeatherService(IWeatherRepository weatherRepoo)
    {
        _weatherRepo = weatherRepo;
    }

    private static WeatherViewModel MapToViewModel(string city, WeatherResponse weather)
    {
        if (weather.Main == null)
            throw new ArgumentException("Weather.Main cannot be null");

        var condition = weather.Weather?.FirstOrDefault();

        return new WeatherViewModel
        {
            City = city,
            Temp = weather.Main.Temp,
            Humidity = weather.Main.Humidity,
            Description = condition?.Description ?? "",
            Icon = condition?.Icon ?? ""
        };
    }

    public async Task<WeatherViewModel?> GetWeatherByCityAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city)) return null;

        var location = await _weatherRepo.GetCoordinatesAsync(city);
        if (location == null) return null;

        var weather = await _weatherRepo.GetWeatherAsync(location.Lat, location.Lon);
        if (weather?.Main == null) return null;

        return MapToViewModel(city, weather);
    }

    public async Task SaveWeatherHistoryAsync(WeatherViewModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.City))
            return;

        var entity = new WeatherHistoryEntity
        {
            PartitionKey = model.City.Trim(),
            RowKey = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm"),
            Temperature = model.Temp,
            Humidity = model.Humidity,
            Condition = model.Description,
            RecordedAt = DateTime.UtcNow,
        };

        await _historyRepo.UpsertDailyAsync(entity);
    }

    public async Task<List<WeatherHistoryEntity>> GetAllHistoryAsync()
    {
        var data = await _historyRepo.GetAllAsync();

        return data?
            .OrderBy(e => e.RecordedAt)
            .ToList() ?? new List<WeatherHistoryEntity>();
    }

    public async Task<List<WeatherHistoryEntity>> GetHistoryByCityAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return new List<WeatherHistoryEntity>();

        var data = await _historyRepo.GetRecentByPartitionKeyAsync(city, 10);

        return (data ?? Enumerable.Empty<WeatherHistoryEntity>())
                .OrderBy(e => e.RecordedAt)
                .ToList();
    }
}