namespace Zucchinimvc.Infrastructure.Repositories.NewsLetterRepo
{
    public interface INewsLetterRepository
    {
     Task<bool> SwitchNewsLetterSubscriptionAsync(string userId);
     Task<List<string>> GetAllUsersEmailsWithActiveNewsLetterAsync();
    }
}