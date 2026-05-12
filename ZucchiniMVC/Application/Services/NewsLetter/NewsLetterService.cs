
using Zucchinimvc.Infrastructure.ApiClients.NewsLetterEmailClient;
using ZucchiniMVC.Application.Services.NewsLetter;

namespace Application.Services.NewsLetter
{
    public class NewsLetterService : INewsLetterService
    {
        private readonly NewsLetterEmailClient _newsLetterEmailClient;
        public NewsLetterService(NewsLetterEmailClient newsLetterEmailClient)
        {
            _newsLetterEmailClient = newsLetterEmailClient;
        }

        public async Task SendNewsLetterEmailAsync(string email, string subject, string content, CancellationToken cancellationToken)
        {
            await _newsLetterEmailClient.SendEmailAsync(email, subject, content, cancellationToken);
        }

    }
}