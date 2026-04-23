using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using Zucchinimvc.Infrastructure.ApiClients.CurrencyClient;
using Zucchinimvc.Infrastructure.Config;
using Zucchinimvc.Models.DTOs.CurrencyDTOs;

namespace Zucchinimvc.Infrastructure.ApiClients.CurrencyClient
{
    public class CurrencyClient
    {

        private readonly HttpClient _http;
        private readonly CurrencySettings _settings;
        private readonly ILogger<CurrencyClient> _logger;


        public CurrencyClient(HttpClient http, IOptions<CurrencySettings> settings, ILogger<CurrencyClient> logger)
        {
            _http = http;
            _settings = settings.Value;
            _logger = logger;
            _http.BaseAddress = new Uri(_settings.BaseUrl);
        }


        public bool IsConfigured => !string.IsNullOrWhiteSpace(_settings.ApiKey);


        public async Task<T?> GetAsync<T>(string endpoint)
        {
            if (!IsConfigured)
            {
                _logger.LogWarning("CurrencyClient: API request to '{Endpoint}' was skipped because the ApiKey is not configured in settings.", endpoint);
                _logger.LogWarning("CurrencyClient: Missing credentials for BaseAddress: {BaseAddress}. Check 'CurrencyApi:ApiKey' in appsettings.json.", _http.BaseAddress);
                return default;
            }

            // Append the API key automatically to every request
            var separator = endpoint.Contains("?") ? "&" : "?";
            var url = $"{endpoint}{separator}apikey={_settings.ApiKey}";

            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
