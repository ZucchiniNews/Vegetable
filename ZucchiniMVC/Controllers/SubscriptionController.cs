using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
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
        var subscriptionTypes = await _subscriptionService.GetAllSubscriptionTypesAsync();
        return View(subscriptionTypes);
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe(int subscriptionTypeId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }
        Subscription subscription = await _subscriptionService.CreateSubscriptionAsync(userId, subscriptionTypeId);
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

