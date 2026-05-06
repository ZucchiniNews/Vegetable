using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.SearchClient
{
    public class SearchClient
    {
        private readonly HttpClient _http;
        private readonly CmsSettings _settings;

        public SearchClient(HttpClient http, IOptions<CmsSettings> settings)
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

        public async Task<string> SearchAsync(string query)
        {
            var response = await _http.GetAsync($"search?query={Uri.EscapeDataString(query)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();

        }
    }
}
