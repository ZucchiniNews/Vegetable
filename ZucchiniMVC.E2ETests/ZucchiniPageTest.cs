using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace ZucchiniMVC.E2ETests;

public class ZucchiniPageTest : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        };
    }

    protected string BaseUrl => 
        Environment.GetEnvironmentVariable("TEST_ENV") == "azure"
        ? "https://zucchinimvc.azurewebsites.net"
        : "Http://localhost:5254";
}
