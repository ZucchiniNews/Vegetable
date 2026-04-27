using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Subscriptions;
using ZucchiniMVC.Application.Services.Payment;

namespace Zucchinimvc.Controllers;

public class SubscriptionController : Controller
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IPaymentService _paymentService;

    public SubscriptionController(ISubscriptionService subscriptionService, IPaymentService paymentService)
    {
        _subscriptionService = subscriptionService;
        _paymentService = paymentService;
    }
    public async Task<IActionResult> Index()
    {
        var plans = await _subscriptionService.GetAllPlansAsync();
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
        Subscription subscription = await _subscriptionService.CreateSubscriptionAsync(userId, planId);
        var session = await _paymentService.CreatePaymentSessionAsync(subscription);
        return Redirect(session.CheckoutUrl);
    }

    [HttpGet("/success")]
    public IActionResult Success(string session_id)
    {
        ViewBag.SessionId = session_id;
        return View();
    }

    [HttpGet("/cancel")]
    public IActionResult Cancel()
    {
        return View();
    }
}

