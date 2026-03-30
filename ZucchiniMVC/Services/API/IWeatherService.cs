using Zucchinimvc.Models.Entities;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Services.API
{
    public interface IWeatherService
    {
        Task<WeatherViewModel?> GetWeatherByCityAsync(string city);
        Task SaveWeatherHistoryAsync(WeatherViewModel model);
        Task<List<WeatherHistoryEntity>> GetWeatherAsync();
        Task<List<WeatherHistoryEntity>> GetHistoryAsync(string city);

    }
}
