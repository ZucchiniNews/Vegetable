using Zucchinimvc.Models.ViewModels;
namespace Zucchinimvc.Application.Services.Analytical
{
    public interface IAnalyticsService
    {
        Task<WeatherViewModel?> GetWeatherAnalyticsAsync(string city);
    }
}