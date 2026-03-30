using Zucchinimvc.Models.ViewModels;
namespace Zucchinimvc.Application.Services.Analytics
{
    public interface IAnalyticsService
    {
        Task<WeatherViewModel?> GetWeatherAnalyticsAsync(string city);
    }
}