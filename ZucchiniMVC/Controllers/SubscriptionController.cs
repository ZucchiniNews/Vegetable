using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Billing;
using Zucchinimvc.Application.Services.Plans;
using Zucchinimvc.Application.Services.Subscriptions;

namespace Zucchinimvc.Controllers;

public class SubscriptionController : Controller
{
    private readonly IPlanService _planService;
    private readonly IBillingService _billingService;
    private readonly ILogger<SubscriptionController> _logger;
    private readonly ISubscriptionService _subscriptionService;


    public SubscriptionController(
        IPlanService planService,
        IBillingService billingService,
        ISubscriptionService subscriptionService,
        ILogger<SubscriptionController> logger)
    {
        _planService = planService;
        _billingService = billingService;
        _logger = logger;
        _subscriptionService = subscriptionService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            var returnUrl = Url.Action("Index", "Subscription");
            return Redirect($"/Identity/Account/Login?returnUrl={Uri.EscapeDataString(returnUrl)}");
        }

        var hasSubscription = await _subscriptionService.UserHasActiveSubscription(userId);

        if (hasSubscription)
        {
            return RedirectToAction("Index", "Home");
        }

        var plans = await _planService.GetAllPlansAsync();
        return View(plans);
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe(int planId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            var returnUrl = Url.Action("Index", "Subscription");
            return Redirect($"/Identity/Account/Login?returnUrl={Uri.EscapeDataString(returnUrl)}");
        }

        var plan = await _planService.FindPlanByIdAsync(planId);
        if (plan == null)
        {
            return NotFound("Plan not found.");
        }

        var session = await _billingService.CreatePaymentSessionAsync(userId, planId);

        return Redirect(session.CheckoutUrl);
    }

    [Route("success")]
    public IActionResult Success(string session_id)
    {
        return View();
    }

    [Route("cancel")]
    public IActionResult Cancel()
    {
        return View();
    }
}