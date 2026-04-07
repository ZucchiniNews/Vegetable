using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Analytics;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Models.ViewModels;


public class AnalyticsService : IAnalyticsService
{
    private readonly IWeatherService _weatherService;

    public AnalyticsService(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    



    //private readonly ICurrencyService _currency;
    //private readonly IUserService _users;

    //public async Task<WeatherAnalyticsVM> GetWeatherAnalytics() { ... }
    //public async Task<CurrencyAnalyticsVM> GetCurrencyAnalytics() { ... }
    //public async Task<UserAnalyticsVM> GetUserAnalytics() { ... }
}