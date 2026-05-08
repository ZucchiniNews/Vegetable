using global::Zucchinimvc.Application.Services.Currency;
using global::Zucchinimvc.Application.Services.Weather;
using Microsoft.AspNetCore.Mvc.Filters;
using Zucchinimvc.Application.Services.CMS;
using Zucchinimvc.Application.Services.Currency;
using Zucchinimvc.Application.Services.Weather;

namespace Zucchinimvc.Infrastructure.ApiFilter;

public class LayoutDataFilter : IAsyncActionFilter
{
    private readonly ICmsService _cmsService;
    private readonly IWeatherService _weatherService;
    private readonly ICurrencyService _currencyService;
    private readonly ILogger<LayoutDataFilter> _logger;

    public LayoutDataFilter(
        ICmsService cmsService,
        IWeatherService weatherService,
        ICurrencyService currencyService,
        ILogger<LayoutDataFilter> logger)
    {
        _cmsService = cmsService;
        _weatherService = weatherService;
        _currencyService = currencyService;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var city = context.HttpContext.Request.Query["city"].ToString();
        string baseCurrency = "USD";

        /*
         maybe later use session to get user preference instead...
            public interface IUserPreferencesService
                {
                    string GetCurrency();
                    string GetCity();
                }
         */

        var cmsTask = _cmsService.GetArticles();

        var weatherTask = _weatherService.GetWeatherByCityAsync(
            string.IsNullOrWhiteSpace(city) ? "Linköping" : city);

        var currencyTask = _currencyService.GetLatestRatesAsync(baseCurrency);

        try
        {
            await Task.WhenAll(cmsTask, weatherTask, currencyTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading layout data");
        }

        context.HttpContext.Items["LayoutCms"] =
            cmsTask.Status == TaskStatus.RanToCompletion
                ? cmsTask.Result
                : null;

        context.HttpContext.Items["LayoutWeather"] =
            weatherTask.Status == TaskStatus.RanToCompletion
                ? weatherTask.Result
                : null;

        context.HttpContext.Items["LayoutCurrency"] =
            currencyTask.Status == TaskStatus.RanToCompletion
                ? currencyTask.Result
                : null;

        await next();
    }
}