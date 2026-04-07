using Microsoft.Extensions.Options;
using System.Text.Json;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.WeatherClient;

public class WeatherClient
{
    private readonly HttpClient _http;
    private readonly WeatherSettings _settings;
    private readonly ILogger<WeatherClient> _logger;

    public WeatherClient(HttpClient http, IOptions<WeatherSettings> settings, ILogger<WeatherClient> logger)
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
            _logger.LogWarning("WeatherClient: API request to '{Endpoint}' was skipped because the ApiKey is not configured in settings.", endpoint);
            _logger.LogWarning("WeatherClient: Missing credentials for BaseAddress: {BaseAddress}. Check 'WeatherApi:ApiKey' in appsettings.json.", _http.BaseAddress);
            return default;
        }

        // Append the API key automatically to every request
        var separator = endpoint.Contains("?") ? "&" : "?";
        var url = $"{endpoint}{separator}appid={_settings.ApiKey}";

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return default;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}

