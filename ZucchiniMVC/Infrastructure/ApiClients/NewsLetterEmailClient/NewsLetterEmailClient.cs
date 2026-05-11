using Resend;

namespace Zucchinimvc.Infrastructure.ApiClients.NewsLetterEmailClient
{
    public class NewsLetterEmailClient
    {
        public IResend Client { get; }

        public NewsLetterEmailClient(IResend resend)
        {
            Client = resend;
        }
    }
}