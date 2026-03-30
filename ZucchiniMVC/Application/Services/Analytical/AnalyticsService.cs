using Zucchinimvc.Application.Services.Analytical;
using Zucchinimvc.Application.Services.Weather;
using Zucchinimvc.Models.ViewModels;


public class AnalyticsService : IAnalyticsService
{
    private readonly IWeatherService _weatherService;

    public AnalyticsService(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<WeatherViewModel?> GetWeatherAnalyticsAsync(string city)
    {

        var targetCity = string.IsNullOrWhiteSpace(city) ? "Linköping" : city;

        var weather = await _weatherService.GetWeatherByCityAsync(targetCity);
        if (weather == null) return null;

        var chartCities = new[] { "Linköping", "Stockholm", "Oslo", "Helsinki", "Copenhagen" };
        weather.Cities = new List<CityWeatherChart>();

        foreach (var cityName in chartCities)
        {
            var history = await _weatherService.GetHistoryAsync(cityName);

            var orderedHistory = history.OrderBy(x => x.RecordedAt).ToList();

            weather.Cities.Add(new CityWeatherChart
            {
                City = cityName,
                Labels = orderedHistory.Select(x => x.RecordedAt.ToString("s")).ToList(),
                Temperatures = orderedHistory.Select(x => x.Temperature).ToList()
            });
        }

        return weather;
    }



    //private readonly ICurrencyService _currency;
    //private readonly IUserService _users;

    //public async Task<WeatherAnalyticsVM> GetWeatherAnalytics() { ... }
    //public async Task<CurrencyAnalyticsVM> GetCurrencyAnalytics() { ... }
    //public async Task<UserAnalyticsVM> GetUserAnalytics() { ... }
}