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
        }

        public async Task<List<NewsletterSubscriberDto>> GetSubscribedUsersAsync()
        {
            var apiKey = _configuration["ZucchiniInternal:ApiKey"];
            using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/internal/users/subscribed");

            request.Headers.Add("X-API-Key", apiKey);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var users = await response.Content
            .ReadFromJsonAsync<List<NewsletterSubscriberDto>>();

            return users ?? new List<NewsletterSubscriberDto>();
        }
    }

}






