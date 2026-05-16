using zucchini_functions.WeeklyNewsLetterEmail.DTOs;

namespace zucchini_functions.Clients.ZucchiniApiClient
{
    public interface IInternalUserClient
    {
        Task<List<NewsletterSubscriberDto>> GetSubscribedUsersAsync();
    }

}
