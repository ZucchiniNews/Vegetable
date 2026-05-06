using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using Zucchinimvc.Infrastructure.ApiClients.CurrencyClient;
using Zucchinimvc.Infrastructure.Repositories.CurrencyRepo;
using Zucchinimvc.Models.DTOs.CurrencyDTOs;

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

        // Service returns DTO, not ViewModel
        public async Task<CurrencyWidgetDto> GetCurrencyWidgetDataAsync(string baseCurrency)
        {
            try
            {
                var allRates = await GetLatestRatesAsync(baseCurrency);

                if (allRates == null || !allRates.Any())
                {
                    _logger.LogWarning("No rates found for {Base}", baseCurrency);
                    return new CurrencyWidgetDto { HasError = true };
                }

                // Filtering logic (business logic stays here)
                var filtered = allRates
                    .Where(x => x.Key == "SEK" || x.Key == "EUR")
                    .ToDictionary(k => k.Key, v => v.Value);

                return new CurrencyWidgetDto
                {
                    Rates = filtered,
                    HasError = filtered.Count == 0,
                    BaseCurrency = baseCurrency
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to build currency widget");
                return new CurrencyWidgetDto { HasError = true };
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