using Microsoft.Extensions.Configuration;
using SharedLib.QueuePublishier.DTOs;
using System.Net.Http.Json;


namespace SharedLib.Clients.ZucchiniApiClient
{

    public class ZucchiniClient : IZucchiniClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ZucchiniClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            var apiKey = _configuration["ZucchiniInternal:ApiKey"];

            _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        }

        public async Task<List<NewsletterSubscriberDto>> GetSubscribedUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/internal/users/subscribed");

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<List<NewsletterSubscriberDto>>()
                ?? new List<NewsletterSubscriberDto>();
        }

        public async Task SaveWeatherHistoryAsync(string city)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/internal/weather/save-history",
                city);

            response.EnsureSuccessStatusCode();
        }
    }

}