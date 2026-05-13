using System.Formats.Tar;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace ZucchiniMVC.E2ETests;

public class SignupTests : ZucchiniPageTest
{
    [Test]
    [Category("WriteOnly")]
    public async Task Signup_Succeeds()
    {
        var email = $"testuser{Guid.NewGuid()}@e2etest.zucchininews.com";

        await Page.GotoAsync($"{BaseUrl}/Identity/Account/Register");
        await Page.Locator("#Input_FirstName").FillAsync("Test");
        await Page.Locator("#Input_LastName").FillAsync("User");
        await Page.Locator("#Input_DateOfBirth").FillAsync("1990-01-01");
        await Page.Locator("#Input_Email").FillAsync(email);
        await Page.Locator("#Input_Password").FillAsync("TestPassword123!");
        await Page.Locator("#Input_ConfirmPassword").FillAsync("TestPassword123!");
        await Page.ClickAsync("button[type='submit']");

        await Expect(Page).ToHaveURLAsync(new Regex(Regex.Escape(BaseUrl.ToLower()) + 
            "/Identity/Account/RegisterConfirmation.*"));
    }
}
