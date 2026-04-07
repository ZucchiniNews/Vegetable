using Azure.Data.Tables;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.ApiClients.AzureTableClient;
using Zucchinimvc.Infrastructure.Repositories;
using Zucchinimvc.Models.DTOs.WeatherDTOs;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Application.Services.Weather;

public class WeatherService : IWeatherService
{
    private readonly IWeatherRepository _weatherRepo;
    private readonly TableClient _historyTable;

    public WeatherService(IWeatherRepository weatherRepo, IAzureTableClient azureTableClient)
    {
        _weatherRepo = weatherRepo;
        _historyTable = azureTableClient.GetClient("ExternalApiHistory");
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

        await _historyTable.UpsertDailyAsync(entity);
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

    public async Task<WeatherViewModel?> GetWeatherAnalyticsAsync(string city)
    {

        var targetCity = string.IsNullOrWhiteSpace(city) ? "Linköping" : city;

        var weather = await GetWeatherByCityAsync(targetCity);
        if (weather == null) return null;

        var chartCities = new[] { "Linköping", "Stockholm", "Oslo", "Helsinki", "Copenhagen" };
        weather.Cities = new List<CityWeatherChart>();

        foreach (var cityName in chartCities)
        {
            var history = await GetHistoryByCityAsync(cityName);

            var orderedHistory = (history ?? new List<WeatherHistoryEntity>())
                                 .OrderBy(x => x.RecordedAt).ToList();

            weather.Cities.Add(new CityWeatherChart
            {
                City = cityName,
                Labels = orderedHistory.Select(x => x.RecordedAt.ToString("s")).ToList(),
                Temperatures = orderedHistory.Select(x => x.Temperature).ToList()
            });
        }

        return weather;
    }
}