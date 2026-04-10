using Application.Services.Logger;
using Infrastructure.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.ApiClients.WeatherClient;

public class WeatherClient : ServiceBase<WeatherClient>
{
    private readonly HttpClient _http;
    private readonly WeatherSettings _settings;
    public WeatherClient(HttpClient http, IOptions<WeatherSettings> settings, ILoggerFactory loggerFactory)
        : base(loggerFactory)  // ← pass to base
    {
        _http = http;
        _settings = settings.Value;
        _http.BaseAddress = new Uri(_settings.BaseUrl);
    }
    public bool IsConfigured => !string.IsNullOrWhiteSpace(_settings.ApiKey);
    public async Task<T?> GetAsync<T>(string endpoint)
    {
        if (!IsConfigured)
        {
            logger.LogWarning("WeatherClient: API request to '{Endpoint}' was skipped because the ApiKey is not configured.", endpoint);
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

