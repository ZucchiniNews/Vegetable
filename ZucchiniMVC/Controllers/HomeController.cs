using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Models;

namespace Zucchinimvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Local()
    {
        return View();
    }
    public IActionResult Sweden()
    {
        return View();
    }
    public IActionResult World()
    {
        return View();
    }
    public IActionResult Sport()
    {
        return View();
    }
    public IActionResult Economey()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
