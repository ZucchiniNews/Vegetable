using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Zucchinimvc.Application.Services.Weather;

namespace ZucchiniBackgroundJobs;

public class WeatherJob
{
    private readonly ILogger _logger;
    private readonly IWeatherService _weatherService;

    public WeatherJob(ILoggerFactory loggerFactory, IWeatherService weatherService)
    {
        _logger = loggerFactory.CreateLogger<WeatherJob>();
        _weatherService = weatherService;
    }

    [Function("WeatherJob")]
    public async Task Run([TimerTrigger("0 0 0,6,12,18 * * *")] TimerInfo myTimer)

    {
        var cities = new[] { "Linköping", "Stockholm", "Oslo", "Helsinki", "Copenhagen" };

        foreach (var city in cities)
        {
            try
            {
                var weather = await _weatherService.GetWeatherByCityAsync(city);
                if (weather != null)
                {
                    await _weatherService.SaveWeatherHistoryAsync(weather);
                    _logger.LogInformation("Saved history for {City}", city);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing weather for {City}", city);
            }
        }
    }
}