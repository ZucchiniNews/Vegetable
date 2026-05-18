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
    private bool _appStarted = false;

    
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
        var projectPath = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "ZucchiniMVC", "ZucchiniMVC.csproj")
        );

        if (Environment.GetEnvironmentVariable("TEST_ENV") == "azure")
            return;

        if (_appStarted) return;
        _appStarted = true;

        _appProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project {projectPath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        };
        _appProcess.Start();

        using var client = new HttpClient();
        for (int i = 0; i < 30; i++)
        {
            try
            {
                var response = await client.GetAsync($"{BaseUrl}");
                if (response.IsSuccessStatusCode) break;
            }
            catch (HttpRequestException ex)
            {
                TestContext.Progress.WriteLine($"Startup check attempt {i + 1}/30 failed: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                TestContext.Progress.WriteLine($"Startup check attempt {i + 1}/30 timed out: {ex.Message}");
            }
            await Task.Delay(1000);
        }    
    }

    [SetUp]
    public void SkipwriteTestsOnProd()
    {
        var isProduction = Environment.GetEnvironmentVariable("TEST_ENV") == "azure";
        var properties = TestContext.CurrentContext.Test.Properties;
        var isWriteTest = properties["Category"]?.Contains("WriteOnly") == true;

        if (isProduction && isWriteTest)
        {
            Assert.Ignore("Write tests are skipped in production (azure) environment.");
        }
    }

    [OneTimeTearDown]
    public async Task CleanUp()
    {
        if (_appStarted)
        {
            _appStarted = false;
            _appProcess?.Kill();
            _appProcess?.Dispose();
        }
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=ZucchiniNewsDb;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ApplicationDbContext(options);
        var testUsers = context.Users
            .Where(u => u.Email != null && u.Email.EndsWith("@e2etest.zucchininews.com"))
            .ToList();

        context.Users.RemoveRange(testUsers);
        await context.SaveChangesAsync();
    }
}
