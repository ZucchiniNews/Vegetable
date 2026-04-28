using Zucchinimvc.Controllers.Stripe.Handlers;

namespace Zucchinimvc.Controllers.Stripe;

public class StripeEventHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public StripeEventHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IStripeEventHandler? GetHandler(string eventType)
    {
        return eventType switch
        {
            "checkout.session.completed" => _serviceProvider.GetService<CheckoutSessionCompletedHandler>(),
            "invoice.paid" => _serviceProvider.GetService<InvoicePaidHandler>(),
            "invoice.payment_failed" => _serviceProvider.GetService<InvoicePaymentFailedHandler>(),
            "customer.subscription.deleted" => _serviceProvider.GetService<CustomerSubscriptionDeletedHandler>(),
            _ => null
        };
    }
}
