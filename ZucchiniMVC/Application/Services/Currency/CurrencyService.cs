
using Zucchinimvc.Infrastructure.Repositories.CurrencyRepo;
using Zucchinimvc.Models.ViewModels;
using System.Globalization;
using Microsoft.Extensions.Logging;

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

        public async Task<CurrencyWidgetViewModel> GetCurrencyWidgetDataAsync(string baseCurrency)
        {
            var viewModel = new CurrencyWidgetViewModel();

            try
            {
                var allRates = await GetLatestRatesAsync(baseCurrency);

                if (allRates == null || !allRates.Any())
                {
                    _logger.LogWarning("No rates found for {Base}", baseCurrency);
                    viewModel.HasError = true;
                    return viewModel;
                }

                
                var filtered = allRates
                    .Where(x => x.Key == "SEK" || x.Key == "EUR")
                    .ToDictionary(k => k.Key, v => v.Value);

                viewModel.Rates = filtered;
                viewModel.HasError = filtered.Count == 0;
                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to build currency widget");
                viewModel.HasError = true;
                return viewModel;
            }
        }

        public async Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency)
        {
            var response = await _currencyRepo.GetLatestRatesAsync(baseCurrency);

            if (response?.Rates == null)
            {
                _logger.LogWarning("Repository returned null rates for {Base}", baseCurrency);
                return new Dictionary<string, decimal>();
            }

            var results = new Dictionary<string, decimal>();

            foreach (var rate in response.Rates)
            {
                if (decimal.TryParse(rate.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var val))
                {
                    results.Add(rate.Key, val);
                }
            }

            return results;
        }
    }
}