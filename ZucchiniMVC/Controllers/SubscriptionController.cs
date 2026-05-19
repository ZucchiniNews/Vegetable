using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZucchiniCore.enums;
using Zucchinimvc.Application.Services.Billing;
using Zucchinimvc.Application.Services.Plans;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Application.Services.UsersService;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Controllers;

public class SubscriptionController : Controller
{
    private readonly IPlanService _planService;
    private readonly IBillingService _billingService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IUserService _userService;


    public SubscriptionController(
        IPlanService planService,
        IBillingService billingService,
        ISubscriptionService subscriptionService,
        IUserService userService
       )
    {
        _planService = planService;
        _billingService = billingService;
        _subscriptionService = subscriptionService;
        _userService = userService;

    }

    [Authorize]
    public async Task<IActionResult> Index(bool showPlans = false)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var subscription = await _subscriptionService.GetLatestSubscriptionForUserAsync(userId);

        // Check if user has a cancelled subscription
        if (subscription?.Status == SubscriptionStatus.Cancelled && !showPlans)
        {
            return View("CancelledSubscription", new SubscriptionPlansViewModel
            {
                CurrentSubscriptionStatus = subscription.Status,
                CurrentPlanName = subscription.PlanId
            });
        }

        // Check if user has an active subscription
        if (subscription?.Status == SubscriptionStatus.Active)
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var plan = await _planService.FindPlanByIdAsync(planId);
        if (plan == null)
            return NotFound("Plan not found.");

        var session = await _billingService.CreatePaymentSessionAsync(userId, planId);

        return Redirect(session.CheckoutUrl);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeNewsletter(bool subscribe)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var result = await _userService.ChangeNewsletterPreferenceAsync(userId, subscribe);

        if (!result.Success && result.StatusType == "error")
            return NotFound();
        TempData["StatusMessage"] = result.StatusMessage;
        TempData["StatusType"] = result.StatusType;
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelSubscription()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var subscription = await _subscriptionService.GetLatestSubscriptionForUserAsync(userId);
        if (subscription == null)
            return NotFound("Subscription not found.");

        // Cancel the subscription in Stripe (and in the local database)
        await _subscriptionService.CancelProviderSubscription(subscription);
        await _subscriptionService.CancelZucchiniSubscription(subscription);

        TempData["StatusMessage"] = "Your subscription has been cancelled.";
        TempData["StatusType"] = "success";

        return RedirectToAction(nameof(Index));
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