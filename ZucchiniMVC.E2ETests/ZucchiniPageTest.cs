using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Data;

namespace ZucchiniMVC.E2ETests;

public class ZucchiniPageTest : PageTest
{
    private Process? _appProcess;

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
        : "http://localhost:5254";

    [OneTimeSetUp]
    public async Task StartApp()
    {
        if (Environment.GetEnvironmentVariable("TEST_ENV") == "azure")
            return;

        _appProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --project C:\\Users\\Student\\Vegetable\\Vegetable\\ZucchiniMVC\\Zucchinimvc.csproj",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        };
        _appProcess.Start();
        await Task.Delay(5000);
    }

    [SetUp]
    public void SkipwriteTestsOnProd()
    {
        var isProduction = Environment.GetEnvironmentVariable("TEST_ENV") == "azure";
        var isWriteTest = TestContext.CurrentContext.Test.Properties
            .ContainsKey("Category") && 
            TestContext.CurrentContext.Test.Properties["Category"]
                .Contains("WriteOnly");

        if (isProduction && isWriteTest)
        {
            Assert.Ignore("Write tests are skipped in production (azure) environment.");
        }
    }

    [OneTimeTearDown]
    public async Task CleanUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=ZucchiniNewsDb;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ApplicationDbContext(options);

        var testUsers = context.Users
            .Where(u => u.Email.EndsWith("@e2etest.zucchininews.com"))
            .ToList();

        context.Users.RemoveRange(testUsers);
        await context.SaveChangesAsync();

        _appProcess?.Kill();
        _appProcess?.Dispose();
    }
}
