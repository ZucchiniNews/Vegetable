using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Repositories.IHistoryRepository;
using Zucchinimvc.Infrastructure.Repositories.WeatherRepo;
using Zucchinimvc.Models.DTOs.WeatherDTOs;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Application.Services.Weather;

public class WeatherService : IWeatherService
{
    private readonly IWeatherRepository _weatherRepo;
    private readonly IHistoryRepository<WeatherHistoryEntity> _historyRepo;

    public WeatherService(IWeatherRepository weatherRepo, IHistoryRepository<WeatherHistoryEntity> historyRepo)
    {
        _weatherRepo = weatherRepo;
        _historyRepo = historyRepo;
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
            RowKey = DateTime.UtcNow.ToString("yyyyMMddHHmm"),
            Temperature = model.Temp,
            Humidity = model.Humidity,
            Condition = model.Description,
            RecordedAt = DateTime.UtcNow,
        };

        await _historyRepo.UpsertAsync(entity);
    }

    public async Task<List<WeatherHistoryEntity>> GetAllHistoryAsync()
    {
        var data = await _historyRepo.GetAllAsync();
        return data?.OrderBy(e => e.RecordedAt).ToList() ?? new List<WeatherHistoryEntity>();
    }

    public async Task<List<WeatherHistoryEntity>> GetHistoryByCityAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city)) return new List<WeatherHistoryEntity>();

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

            weather.Cities.Add(new CityWeatherChart
            {
                City = cityName,
                Labels = history.Select(x => x.RecordedAt.ToString("s")).ToList(),
                Temperatures = history.Select(x => x.Temperature).ToList()
            });
        }

        return weather;
    }

    private static WeatherViewModel MapToViewModel(string city, WeatherResponse weather)
    {
        var condition = weather.Weather?.FirstOrDefault();
        return new WeatherViewModel
        {
            City = city,
            Temp = weather.Main!.Temp,
            Humidity = weather.Main.Humidity,
            Description = condition?.Description ?? "",
            Icon = condition?.Icon ?? ""
        };
    }
}