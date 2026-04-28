using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using System.Text;
using Zucchinimvc.Infrastructure.Config;
using Zucchinimvc.Controllers.Stripe;

[ApiController]
[Route("api/stripe/webhook")]
public class StripeWebhookController : ControllerBase
{
    private readonly ILogger<StripeWebhookController> _logger;
    private readonly StripeEventHandlerFactory _handlerFactory;
    private readonly string _webhookSecret;

    public StripeWebhookController(
        ILogger<StripeWebhookController> logger,
        StripeEventHandlerFactory handlerFactory,
        IOptions<StripeSettings> stripeOptions)
    {
        _logger = logger;
        _handlerFactory = handlerFactory;
        _webhookSecret = stripeOptions.Value.WebhookSecret;
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        string json;
        using (var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
        {
            json = await reader.ReadToEndAsync();
        }

        var stripeSignature = Request.Headers["Stripe-Signature"];
        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _webhookSecret);
        }
        catch (StripeException ex)
        {
            _logger.LogWarning(ex, "Stripe webhook signature validation failed.");
            return BadRequest();
        }

        var handler = _handlerFactory.GetHandler(stripeEvent.Type);
        if (handler == null)
        {
            _logger.LogInformation("Unhandled Stripe event type: {Type}", stripeEvent.Type);
            return Ok();
        }

        try
        {
            await handler.HandleAsync(stripeEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling Stripe event {Type}", stripeEvent.Type);
            return StatusCode(500);
        }

        return Ok();
    }
}