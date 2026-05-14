using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Billing;
using Zucchinimvc.Application.Services.Plans;
using Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue;
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
    private readonly INewsLetterQueuePublisher _newsLetterQueuePublisher;


    public SubscriptionController(
        IPlanService planService,
        IBillingService billingService,
        ISubscriptionService subscriptionService,
        IUserService userService,
        INewsLetterQueuePublisher newsLetterQueuePublisher)
    {
        _planService = planService;
        _billingService = billingService;
        _subscriptionService = subscriptionService;
        _userService = userService;
        _newsLetterQueuePublisher = newsLetterQueuePublisher;

    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

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
        var user = await _userService.GetUserByIdAsync(userId);

        if (user == null)
            return NotFound();

        var currentState = user.NewsletterSubscribed;
        if (currentState == subscribe)
        {
            TempData["StatusMessage"] = subscribe
                ? "You are already subscribed to the newsletter."
                : "You are already unsubscribed from the newsletter.";
            TempData["StatusType"] = "info";
            return RedirectToAction(nameof(Index));
        }

        await _userService.UpdateNewsletterPreferenceAsync(userId, subscribe);

        if (subscribe)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                TempData["StatusMessage"] = "Newsletter subscription enabled, but no welcome email was queued because your account does not have an email address.";
            }
            else
            {
                var message = new NewsLetterQueueMessage
                {
                    Email = user.Email,
                    Subject = "Welcome to our Newsletter!",
                    HtmlBody = "<h1>Welcome to our Newsletter!</h1><p>Thank you for subscribing.</p>"
                };

                await _newsLetterQueuePublisher.PublishAsync(message, HttpContext.RequestAborted);
                TempData["StatusMessage"] = "Newsletter subscription enabled. A welcome email has been queued.";
            }
        }
        else
        {
            TempData["StatusMessage"] = "Newsletter subscription disabled.";
        }

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