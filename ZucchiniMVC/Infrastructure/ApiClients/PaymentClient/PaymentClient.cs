
using Microsoft.Extensions.Options;
using Stripe;

namespace ZucchiniMVC.Infrastructure.ApiClients.PaymentClient
{
    public class PaymentClient
    {
        public StripeClient Client { get; }
        public string SuccessUrl { get; }
        public string CancelUrl { get; }

        public PaymentClient(IOptions<StripeSettings> stripeOptions)
        {
            var settings = stripeOptions.Value;
            Client = new StripeClient(settings.SecretKey);
            SuccessUrl = settings.SuccessUrl;
            CancelUrl = settings.CancelUrl;
        }
    }
}
