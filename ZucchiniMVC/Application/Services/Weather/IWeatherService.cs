
using ZucchiniCore.Entities;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Application.Services.Weather
{
    public interface IWeatherService
    {
        Task<WeatherViewModel?> GetWeatherByCityAsync(string city);
        Task SaveWeatherHistoryAsync(WeatherViewModel model);
        Task<List<WeatherHistoryEntity>> GetAllHistoryAsync();
        Task<List<WeatherHistoryEntity>> GetHistoryByCityAsync(string city);

    }
}
