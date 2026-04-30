using System.Text.Json;
using Zucchinimvc.Models.DTOs.CurrencyDTOs;

namespace Zucchinimvc.Infrastructure.ApiClients.CurrencyClient
{
    public interface CurrencyClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CurrencyApiClient> _logger;
        private readonly string _apiKey;

        public CurrencyApiClient(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<CurrencyApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = configuration["CurrencyApi:ApiKey"] ?? "113a167a80dc42b09c2e14cdc008f2e3";
            _httpClient.BaseAddress = new Uri(configuration["CurrencyApi:BaseUrl"] ?? "https://api.currencyfreaks.com/v2.0/");
        }

        public async Task<CurrencyRateDto> GetLatestRatesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"rates/latest?apikey={_apiKey}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var rates = JsonSerializer.Deserialize<CurrencyRateDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return rates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching latest currency rates");
                throw;
            }
        }

        public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            var rates = await GetLatestRatesAsync();

            if (rates.Rates.ContainsKey(fromCurrency) && rates.Rates.ContainsKey(toCurrency))
            {
                var fromRate = rates.Rates[fromCurrency];
                var toRate = rates.Rates[toCurrency];
                return toRate / fromRate;
            }

            throw new ArgumentException("Currency not found");
        }

        public async Task<decimal> ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            var rate = await GetExchangeRateAsync(fromCurrency, toCurrency);
            return amount * rate;
        }
    }

}
}