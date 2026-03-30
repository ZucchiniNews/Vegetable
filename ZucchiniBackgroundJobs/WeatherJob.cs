using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Zucchinimvc.Application.Services.Weather;

namespace ZucchiniBackgroundJobs;

public class WeatherJob
{
    private readonly ILogger _logger;
    private readonly WeatherService _weatherService;

    public WeatherJob(ILoggerFactory loggerFactory, WeatherService weatherService)
    {
        _logger = loggerFactory.CreateLogger<WeatherJob>();
        _weatherService = weatherService;
    }

    [Function("WeatherJob")]
    public async Task Run([TimerTrigger("0 0 0,6,12,18 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("Weather job started at: {time}", DateTime.Now);

        var cities = new[] { "Linköping", "Stockholm", "Oslo", "Helsinki", "Copenhagen" };

        var tasks = cities.Select(city => _weatherService.GetWeatherByCityAsync(city));

        await Task.WhenAll(tasks);

        _logger.LogInformation("Weather job finished");

        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next run: {next}", myTimer.ScheduleStatus.Next);
        }
    }
}