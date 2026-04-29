using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Billing;
using Zucchinimvc.Application.Services.Plans;

namespace Zucchinimvc.Controllers;

public class SubscriptionController : Controller
{
    private readonly IPlanService _planService;
    private readonly IBillingService _billingService;
    private readonly ILogger<SubscriptionController> _logger;


    public SubscriptionController(
        IPlanService planService,
        IBillingService billingService,
        ILogger<SubscriptionController> logger)
    {
        _planService = planService;
        _billingService = billingService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var plans = await _planService.GetAllPlansAsync();
        return View(plans);
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe(int planId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }
        var plan = await _planService.FindPlanByIdAsync(planId);
        if (plan == null)
        {
            return NotFound("Plan not found.");
        }
        var billing = await _billingService.GetOrCreateStripeCustomerAsync(userId);
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