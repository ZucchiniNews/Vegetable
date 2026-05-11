using Microsoft.Extensions.Options;
using Resend;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.NewsLetterEmailClient
{
    public class NewsLetterEmailClient
    {
        private readonly NewsLetterSettings _settings;

        public ResendClient Client { get; }

        public NewsLetterEmailClient(
            IOptions<NewsLetterSettings> settings,
            ResendClient client)
        {
            _settings = settings.Value;
            Client = client;
        }

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(_settings.ApiKey);
    }
}