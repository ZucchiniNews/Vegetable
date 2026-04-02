using Microsoft.Extensions.Options;
using System.Text.Json;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.OpenWeatherClient;

public class WeatherClient
{
    private readonly HttpClient _http;
    private readonly OpenWeatherSettings _settings;
    private readonly JsonSerializerOptions _jsonOptions;

    public WeatherClient(HttpClient http, IOptions<OpenWeatherSettings> settings)
    {
        _http = http;
        _settings = settings.Value;
        _http.BaseAddress = new Uri(_settings.BaseUrl);
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(_settings.ApiKey);
    public async Task<T?> GetAsync<T>(string endpoint)
    {
        if (!IsConfigured)
        {
            // Log a warning here if you want!
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

