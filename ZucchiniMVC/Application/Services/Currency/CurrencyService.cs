using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Infrastructure.Repositories.CurrencyRepo;
using Zucchinimvc.Models.ViewModels;
using System.Globalization;

namespace Zucchinimvc.Application.Services.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILogger<CurrencyService> _logger;
        private readonly ICurrencyRepository _currencyRepo;

        public CurrencyService(ILogger<CurrencyService> logger, ICurrencyRepository currencyRepo)
        {
            _logger = logger;
            _currencyRepo = currencyRepo;
        }

        // NEW METHOD FOR CURRENCY WIDGET
        public async Task<CurrencyWidgetViewModel> GetCurrencyWidgetDataAsync(string baseCurrency)
        {
            var viewModel = new CurrencyWidgetViewModel();

            try
            {
                var usdRates = await GetLatestRatesAsync(baseCurrency);

                if (usdRates == null || usdRates.Count == 0)
                {
                    viewModel.HasError = true;
                    _logger.LogWarning("No currency rates returned for widget");
                    return viewModel;
                }

                var sekEurRates = new Dictionary<string, decimal>();

                if (usdRates.ContainsKey("SEK"))
                    sekEurRates.Add("SEK", usdRates["SEK"]);

                if (usdRates.ContainsKey("EUR"))
                    sekEurRates.Add("EUR", usdRates["EUR"]);

                if (sekEurRates.Count == 0)
                {
                    viewModel.HasError = true;
                    _logger.LogWarning("SEK/EUR not found for widget");
                    return viewModel;
                }

                viewModel.Rates = sekEurRates;
                viewModel.HasError = false;

                return viewModel;
            }
            catch (Exception ex)
            {
                viewModel.HasError = true;
                _logger.LogError(ex, "Error getting currency widget data");
                return viewModel;
            }
        }

        public async Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency)
        {
            var response = await _currencyRepo.GetLatestRatesAsync(baseCurrency);

            if (response == null || response.Rates == null)
            {
                _logger.LogWarning("Response or Rates is null for {BaseCurrency}", baseCurrency);
                return new Dictionary<string, decimal>();
            }

            _logger.LogInformation($"Received {response.Rates.Count} rates from API");

            var decimalRates = new Dictionary<string, decimal>();
            foreach (var rate in response.Rates)
            {
               
                if (decimal.TryParse(rate.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue))
                {
                    decimalRates.Add(rate.Key, decimalValue);
                }
                else
                {
                    _logger.LogWarning($"Failed to parse rate for {rate.Key}: '{rate.Value}'");
                }
            }

            _logger.LogInformation($"Successfully parsed {decimalRates.Count} rates");

            
            if (decimalRates.ContainsKey("SEK"))
                _logger.LogInformation($"SEK rate: {decimalRates["SEK"]}");
            if (decimalRates.ContainsKey("EUR"))
                _logger.LogInformation($"EUR rate: {decimalRates["EUR"]}");

            return decimalRates;
        }

        public async Task<Dictionary<string, decimal>> GetRatesAsync(string toCurrency)
        {
            var currency = toCurrency.ToUpper().Trim();

            var rates = await GetLatestRatesAsync(currency);

            if (rates.Count == 0)
            {
                _logger.LogWarning("No rates found for {Currency}", currency);
                throw new KeyNotFoundException($"Rates for {currency} not found.");
            }

            return rates;
        }

        public async Task<ActionResult<Dictionary<string, decimal>>> GetEURRates()
        {
            var rates = await GetLatestRatesAsync("EUR");

            if (rates.Count == 0)
            {
                _logger.LogWarning("No rates found for EUR");
                throw new KeyNotFoundException($"Rates for EUR not found.");
            }

            return new ActionResult<Dictionary<string, decimal>>(rates);
        }

        public async Task<ActionResult<Dictionary<string, decimal>>> GetSEKRates()
        {
            var rates = await GetLatestRatesAsync("SEK");

            if (rates.Count == 0)
            {
                _logger.LogWarning("No rates found for SEK");
                throw new KeyNotFoundException($"Rates for SEK not found.");
            }

            return new ActionResult<Dictionary<string, decimal>>(rates);
        }
    }
}