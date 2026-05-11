using System.Globalization;
using Zucchinimvc.Infrastructure.Repositories.CurrencyRepo;
using Microsoft.Extensions.Caching.Memory;

namespace Zucchinimvc.Application.Services.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILogger<CurrencyService> _logger;
        private readonly ICurrencyRepository _currencyRepo;
        private readonly IMemoryCache _cache;

        public CurrencyService(ILogger<CurrencyService> logger, ICurrencyRepository currencyRepo, IMemoryCache cache)
        {
            _logger = logger;
            _currencyRepo = currencyRepo;
            _cache = cache;
        }

        public async Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency)
        {
            var cacheKey = $"currency_rates_{baseCurrency}";

            if (_cache.TryGetValue(cacheKey, out Dictionary<string, decimal> cachedRates))
            {
                _logger.LogInformation("Cache hit for {Base}", baseCurrency);
                return cachedRates;
            }

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

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            _cache.Set(cacheKey, results, cacheOptions);
            _logger.LogInformation("Cache set for {Base}", baseCurrency);

            return results;
        }
    }
}