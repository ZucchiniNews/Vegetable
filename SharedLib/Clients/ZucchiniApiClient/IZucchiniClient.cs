using SharedLib.QueuePublishier.DTOs;

namespace SharedLib.Clients.ZucchiniApiClient
{
    public interface IZucchiniClient
    {
        Task<List<NewsletterSubscriberDto>> GetSubscribedUsersAsync();

        Task SaveWeatherHistoryAsync(string city);
    }

}
