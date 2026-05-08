using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using Zucchinimvc.Infrastructure.Config;


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
                new AuthenticationHeaderValue("Bearer", _settings.Token);
        }
    }

    public async Task<T> GetAsync<T>(string endpoint,
    CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

        if (!string.IsNullOrWhiteSpace(_settings.Token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.Token);
        }

        var response = await _http.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        using var jsonDocument = JsonDocument.Parse(json);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
        {
            return dataElement.Deserialize<T>(options)!;
        }

        return JsonSerializer.Deserialize<T>(json, options)!;
    }
}