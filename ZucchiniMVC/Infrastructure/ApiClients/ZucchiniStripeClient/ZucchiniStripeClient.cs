using Microsoft.Extensions.Options;
using Stripe;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient
{
    public class ZucchiniStripeClient
    {
        public StripeClient Client { get; }
        public StripeSettings Settings { get; }

        public ZucchiniStripeClient(IOptions<StripeSettings> stripeOptions)
        {
            Settings = stripeOptions.Value;
            Client = new StripeClient(Settings.SecretKey);
        }

        public StripeClient GetStripeClient()
        {
            return Client;
        }
    }
}