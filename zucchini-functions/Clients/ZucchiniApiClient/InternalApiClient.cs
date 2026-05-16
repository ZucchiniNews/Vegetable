using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using zucchini_functions.WeeklyNewsLetterEmail.DTOs;

namespace zucchini_functions.Clients.ZucchiniApiClient
{

    public class InternalUserClient : IInternalUserClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public InternalUserClient(
        HttpClient httpClient,
        IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<NewsletterSubscriberDto>> GetSubscribedUsersAsync()
        {
            var apiKey = _configuration["InternalApiKey"];

            var request = new HttpRequestMessage(
            HttpMethod.Get,
            "internal/users/subscribed");

            request.Headers.Add("X-API-Key", apiKey);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var users = await response.Content
            .ReadFromJsonAsync<List<NewsletterSubscriberDto>>();

            return users ?? new List<NewsletterSubscriberDto>();
        }
    }

}






