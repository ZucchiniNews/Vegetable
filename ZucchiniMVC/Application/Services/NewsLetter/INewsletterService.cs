
namespace ZucchiniMVC.Application.Services.NewsLetter
{
    public interface INewsLetterService
    {
        Task<bool> SwitchNewsLetterSubscriptionAsync(string userId);
        Task<List<string>> GetAllUsersWithActiveNewsLetterSubscriptionAsync();
    }
}