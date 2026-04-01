using Infrastrcture.ApiClients.StrapiClient;
using Microsoft.Extensions.Options;
using System.Text.Json;

public class StrapiClient : IStrapiClient
{
    private readonly HttpClient _http;
    private readonly StrapiSettings _settings;

    public StrapiClient(HttpClient http, IOptions<StrapiSettings> settings)
    {
        _http = http;
        _settings = settings.Value;
    }

    public async Task<StrapiResponse<ArticleDto>> GetArticlesAsync(string endpoint)
    {
        _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.Token);
        var url = $"{_settings.BaseUrl}{endpoint}";
        var res = await _http.GetAsync(url);
        res.EnsureSuccessStatusCode();

        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<StrapiResponse<ArticleDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}