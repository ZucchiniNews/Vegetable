using Microsoft.Extensions.Options;
using System.Text.Json;
using Infrastructure.Config;

namespace Infrastructure.ApiClients.CmsClient;

public class CmsClient
{
    private readonly HttpClient _http;
    private readonly CmsSettings _settings;

    public CmsClient(HttpClient http, IOptions<CmsSettings> settings)
    {
        _http = http;
        _settings = settings.Value;

        _http.BaseAddress = new Uri(_settings.BaseUrl);

        if (!string.IsNullOrWhiteSpace(_settings.Token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.Token);
        }
    }

    public async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _http.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        using var jsonDocument = JsonDocument.Parse(json);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (jsonDocument.RootElement.ValueKind == JsonValueKind.Object && jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
        {
            return dataElement.Deserialize<T>(options)!;
        }

        return JsonSerializer.Deserialize<T>(json, options)!;
    }
}