using Zucchinimvc.Models.ViewModels;
using Zucchinimvc.Services.API;

namespace Zucchinimvc.Services
{
    public interface IAnalyticsService
    {
        Task<WeatherViewModel?> GetWeatherAnalyticsAsync(string city);
    }
}