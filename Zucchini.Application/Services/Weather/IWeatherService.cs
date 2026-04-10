using Domain.Entities;
// using Zucchini.Presentation.Models.ViewModels;  // no referencing to presentaion

namespace Application.Services.Weather;

public interface IWeatherService
{
    Task<WeatherSnapshot?> GetWeatherByCityAsync(string city);
    Task SaveWeatherHistoryAsync(WeatherHistory model);
    Task<List<WeatherHistory>> GetAllHistoryAsync();
    Task<List<WeatherHistory>> GetHistoryByCityAsync(string city);
    Task<WeatherHistory?> GetWeatherAnalyticsAsync(string city);
}