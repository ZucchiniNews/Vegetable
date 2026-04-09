using Zucchini.Domain.Entities;
// using Zucchini.Presentation.Models.ViewModels;  // no referencing to presentaion

namespace Zucchini.Application.Services.Weather;

public interface IWeatherService
{
    Task<WeatherViewModel?> GetWeatherByCityAsync(string city);
    Task SaveWeatherHistoryAsync(WeatherViewModel model);
    Task<List<WeatherHistoryEntity>> GetAllHistoryAsync();
    Task<List<WeatherHistoryEntity>> GetHistoryByCityAsync(string city);
    Task<WeatherViewModel?> GetWeatherAnalyticsAsync(string city);
}