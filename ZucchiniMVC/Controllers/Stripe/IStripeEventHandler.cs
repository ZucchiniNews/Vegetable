using Stripe;

namespace Zucchinimvc.Controllers.Stripe;

public interface IStripeEventHandler
{
    Task HandleAsync(Event stripeEvent);
}
