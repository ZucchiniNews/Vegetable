using Microsoft.Extensions.Options;
using System.Text.Json;
using Zucchinimvc.Infrastructure.ApiClients.StrapiClient;
using Zucchinimvc.Infrastructure.Config;

public class StrapiClient : IStrapiClient
{
    private readonly HttpClient _http;
    private readonly StrapiSettings _settings;

    public StrapiClient(HttpClient http, IOptions<StrapiSettings> settings)
    {
        _http = http;
        _settings = settings.Value;
    }

    public async Task<StrapiResponse<T>> GetAsync<T>(string endpoint)
    {
        var url = $"{_settings.BaseUrl}{endpoint}";
        var res = await _http.GetAsync(url);
        res.EnsureSuccessStatusCode();

        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<StrapiResponse<T>>(json);
    }
}