using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Plans;
using Zucchinimvc.Application.Services.Subscriptions;

namespace Zucchinimvc.Controllers;

public class SubscriptionController : Controller
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IPlanService _planService;
    private readonly ILogger<SubscriptionController> _logger;

    public SubscriptionController(
        ISubscriptionService subscriptionService,
        IPlanService planService,
        ILogger<SubscriptionController> logger)
    {
        _subscriptionService = subscriptionService;
        _planService = planService;
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
        var session = await _subscriptionService.CreatePaymentSessionAsync(userId, planId);
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