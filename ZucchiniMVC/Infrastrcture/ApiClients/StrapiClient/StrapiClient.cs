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
    private async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _http.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }


    public async Task<IEnumerable<ArticleDto>> GetArticlesAsync<T>(string endpoint)
    {
        var result = await GetAsync<IEnumerable<ArticleDto>>(endpoint);
        return result;
    }

}