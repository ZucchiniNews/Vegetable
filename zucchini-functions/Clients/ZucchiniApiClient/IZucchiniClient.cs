
using SharedLib.DTOs.NewsLetterSubscriber;

namespace zucchini_functions.Clients.ZucchiniApiClient
{
    public interface IZucchiniClient
    {
        Task<List<NewsletterSubscriberDto>> GetSubscribedUsersAsync();
    }

}
