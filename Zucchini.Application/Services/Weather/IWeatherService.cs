using Zucchini.Domain.Entities;
using Zucchini.Application.Models.ViewModels;

namespace Zucchini.Application.Services.Weather;

public interface IWeatherService
{
    Task<WeatherViewModel?> GetWeatherByCityAsync(string city);
    Task SaveWeatherHistoryAsync(WeatherViewModel model);
    Task<List<WeatherHistoryEntity>> GetAllHistoryAsync();
    Task<List<WeatherHistoryEntity>> GetHistoryByCityAsync(string city);
    Task<WeatherViewModel?> GetWeatherAnalyticsAsync(string city);
}