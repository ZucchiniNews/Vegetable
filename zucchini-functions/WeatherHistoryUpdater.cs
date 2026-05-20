using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SharedLib.Clients.ZucchiniApiClient;
using System.Net.Http;

namespace zucchini_functions;

internal class WeatherHistoryUpdater
{
    private readonly ILogger _logger;
    private readonly IZucchiniClient _zucchiniClient;

    public WeatherHistoryUpdater(ILoggerFactory loggerFactory, IZucchiniClient zucchiniClient)
    {
        _logger = loggerFactory.CreateLogger<WeatherHistoryUpdater>();
        _zucchiniClient = zucchiniClient;
    }

    [Function("SaveWeatherHistory")]
    public async Task Run([TimerTrigger("0 0 0,6,12,18 * * *")] TimerInfo myTimer)

    {
        var cities = new[] { "Linköping", "Stockholm", "Oslo", "Helsinki", "Copenhagen" };

        foreach (var city in cities)
        {
            try
            {
                await _zucchiniClient.SaveWeatherHistoryAsync(city);
                    _logger.LogInformation("Saved history for {City}", city);
                
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error processing weather for {City}", city);
            }
        }
    }

}
