using Zucchinimvc.Infrastructure.ApiClients.NewsLetterClient;
using ZucchiniMVC.Application.Services.NewsLetter;

namespace Zucchinimvc.Application.Services.NewsLetter
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