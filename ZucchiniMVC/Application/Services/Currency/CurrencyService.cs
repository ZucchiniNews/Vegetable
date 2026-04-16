using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Infrastructure.Repositories.CurrencyRepo;

namespace Zucchinimvc.Application.Services.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CurrencyService> _logger;
        private readonly ICurrencyRepository _currencyRepo;

        public CurrencyService(HttpClient httpClient, ILogger<CurrencyService> logger, ICurrencyRepository currencyRepo)
        {
            _httpClient = httpClient;
            _logger = logger;
            _currencyRepo = currencyRepo;
        }

        public async Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency)
        {
            var response = await _currencyRepo.GetLatestRatesAsync(baseCurrency);

            if (response == null || response.Rates == null)
                return new Dictionary<string, decimal>();

            return response.Rates;
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

            return rates;
        }

        public async Task<ActionResult<Dictionary<string, decimal>>> GetSEKRates()
        {
            var rates = await GetLatestRatesAsync("SEK");

            if (rates.Count == 0)
            {
                _logger.LogWarning("No rates found for SEK");
                throw new KeyNotFoundException($"Rates for SEK not found.");
            }

            return rates;
        }
    }

}
