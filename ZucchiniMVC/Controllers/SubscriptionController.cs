using Microsoft.AspNetCore.Mvc;

namespace Zucchinimvc.Controllers;

public class SubscriptionController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Subscribe(string subscriptionModel)
    {
        return RedirectToAction("Index", "Home");
    }
}

