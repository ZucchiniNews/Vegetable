


using Zucchinimvc.Infrastructure.ApiClients.NewsLetterEmailClient;
using Zucchinimvc.Infrastructure.Repositories.NewsLetterRepo;
using ZucchiniMVC.Application.Services.NewsLetter;

namespace Application.Services.NewsLetter
{
    public class NewsLetterService : INewsLetterService
    {
        private readonly INewsLetterRepository _newsLetterRepository;
        private readonly NewsLetterEmailClient _newsLetterEmailClient;
        public NewsLetterService(INewsLetterRepository newsLetterRepository, NewsLetterEmailClient newsLetterEmailClient)
        {
            _newsLetterRepository = newsLetterRepository;
            _newsLetterEmailClient = newsLetterEmailClient;
        }

        public async Task<bool> SwitchNewsLetterSubscriptionAsync(string userId)
        {
            return await _newsLetterRepository.SwitchNewsLetterSubscriptionAsync(userId);
        }

        public async Task<List<string>> GetAllUsersWithActiveNewsLetterSubscriptionAsync()
        {
            return await _newsLetterRepository.GetAllUsersEmailsWithActiveNewsLetterAsync();
        }

        public async Task SendNewsLetterEmailAsync(string email, string subject, string content)
        {
            await _newsLetterEmailClient.SendEmailAsync(email, subject, content);
        }

    }
}