using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Billing;
using Zucchinimvc.Application.Services.Plans;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Controllers;

public class SubscriptionController : Controller
{
    private readonly IPlanService _planService;
    private readonly IBillingService _billingService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<SubscriptionController> _logger;

    public SubscriptionController(
        IPlanService planService,
        IBillingService billingService,
        ISubscriptionService subscriptionService,
        ILogger<SubscriptionController> logger)
    {
        _planService = planService;
        _billingService = billingService;
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var hasSubscription = await _subscriptionService
            .UserHasActiveSubscription(userId);

        if (hasSubscription)
        {
            return View("AlreadySubscribed");
        }

        var plans = await _planService.GetAllPlansAsync();

        return View(new SubscriptionPlansViewModel
        {
            Plans = plans
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Subscribe(int planId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var plan = await _planService.FindPlanByIdAsync(planId);
        if (plan == null)
            return NotFound("Plan not found.");

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