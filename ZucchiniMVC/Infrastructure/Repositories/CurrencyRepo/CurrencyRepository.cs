using Microsoft.Extensions.Caching.Memory;
using Zucchinimvc.Infrastructure.ApiClients.CurrencyClient;
using Zucchinimvc.Models.DTOs.CurrencyDTOs;
using Zucchinimvc.Services;


namespace Zucchinimvc.Infrastructure.Repositories.CurrencyRepo
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ICurrencyApiClient _apiClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CurrencyRepository> _logger;
        private const string CACHE_KEY = "currency_rates";
        private const int CACHE_DURATION_MINUTES = 60; // Cache for 1 hour

        public CurrencyRepository(
            ICurrencyApiClient apiClient,
            IMemoryCache cache,
            ILogger<CurrencyRepository> logger)
        {
            _apiClient = apiClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<CurrencyRateDto> GetCachedRatesAsync()
        {
            if (_cache.TryGetValue(CACHE_KEY, out CurrencyRateDto cachedRates))
            {
                return cachedRates;
            }

            return await UpdateRatesCacheAndGetAsync();
        }

        private async Task<CurrencyRateDto> UpdateRatesCacheAndGetAsync()
        {
            try
            {
                var rates = await _apiClient.GetLatestRatesAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES))
                    .SetPriority(CacheItemPriority.High);

                _cache.Set(CACHE_KEY, rates, cacheOptions);

                return rates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update currency rates cache");
                throw;
            }
        }

        public async Task<ExchangeRateDto> GetExchangeRateAsync(string currencyCode)
        {
            var rates = await GetCachedRatesAsync();

            if (rates.Rates.ContainsKey(currencyCode))
            {
                return new ExchangeRateDto
                {
                    CurrencyCode = currencyCode,
                    Rate = rates.Rates[currencyCode],
                    LastUpdated = DateTime.Parse(rates.Date)
                };
            }

            throw new ArgumentException($"Currency {currencyCode} not found");
        }

        public async Task<List<ExchangeRateDto>> GetAllExchangeRatesAsync()
        {
            var rates = await GetCachedRatesAsync();

            return rates.Rates.Select(r => new ExchangeRateDto
            {
                CurrencyCode = r.Key,
                Rate = r.Value,
                LastUpdated = DateTime.Parse(rates.Date)
            }).ToList();
        }

        public async Task UpdateRatesCacheAsync()
        {
            await UpdateRatesCacheAndGetAsync();
        }

        public async Task<decimal> ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            var fromRate = await GetExchangeRateAsync(fromCurrency);
            var toRate = await GetExchangeRateAsync(toCurrency);

            return amount * (toRate.Rate / fromRate.Rate);
        }

        public Task<CurrencyRateDto> GetLatestRatesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ExchangeRateDto> GetExchangeRatesAsync(string currencyCode)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            throw new NotImplementedException();
        }
    }
}
