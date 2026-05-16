
using SharedLib.DTOs.NewsLetterSubscriber;

namespace zucchini_functions.Clients.ZucchiniApiClient
{
    public interface IInternalUserClient
    {
        Task<List<NewsletterSubscriberDto>> GetSubscribedUsersAsync();
    }

}
