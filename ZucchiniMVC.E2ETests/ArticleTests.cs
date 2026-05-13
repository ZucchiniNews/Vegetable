using System.Text.RegularExpressions;
using Microsoft.Playwright; 

namespace ZucchiniMVC.E2ETests;

public class ArticleTests : ZucchiniPageTest
{
    [Test]
    public async Task FreeArticle_IsAccessibleToGuest()
    {
        await Page.GotoAsync($"{BaseUrl}/Article/free-article");
        await Expect(Page).ToHaveTitleAsync(new Regex("ZucchiniNews"));
    }

    [Test]
    public async Task PremiumArticle_IsNotAccessibleToGuest()
    {
        await Page.GotoAsync($"{BaseUrl}/Article/Nicotine-Promise-Cognitive-ADHD-informational-neurodevelopmental");
        await Expect(Page).ToHaveTitleAsync(new Regex("ZucchiniNews"));
        await Expect(Page.GetByText("This content is for subscribers only")).ToBeVisibleAsync();
    }

    [Test]
    public async Task PremiumArticle_ShowsPaywall_ForAuthenticatedUserWithoutSubscription()
    {
        await Page.GotoAsync($"{BaseUrl}/Identity/Account/Login");
        await Page.GetByLabel("Email").FillAsync("testuser@zucchinews.com");
        await Page.GetByLabel("Password").FillAsync("TestPassword123!");
        await Page.ClickAsync("button[type='submit']");
        await Page.GotoAsync($"{BaseUrl}/Article/Nicotine-Promise-Cognitive-ADHD-informational-neurodevelopmental");
        await Expect(Page).ToHaveTitleAsync(new Regex("ZucchiniNews"));
        await Expect(Page.GetByText("This content is for subscribers only")).ToBeVisibleAsync();
    }

    [Test]
    public async Task PremiumArticle_IsAccessibleToAuthenticatedUserWithSubscription()
    {
        await Page.GotoAsync($"{BaseUrl}/Identity/Account/Login");
        await Page.GetByLabel("Email").FillAsync("testuser1@zucchinews.com");
        await Page.GetByLabel("Password").FillAsync("TestPassword123!");
        await Page.ClickAsync("button[type='submit']");
        await Page.GotoAsync($"{BaseUrl}/Article/Nicotine-Promise-Cognitive-ADHD-informational-neurodevelopmental");
        await Expect(Page).ToHaveTitleAsync(new Regex("ZucchiniNews"));
        await Expect(Page.GetByText("This content is for subscribers only")).ToBeHiddenAsync();
    }

    [Test]
    public async Task ArticleView_DisplaysViewCount()
    {
        await Page.GotoAsync($"{BaseUrl}/Article/free-article");

        var viewCount = await Page.Locator(".view-count").InnerTextAsync();

        Assert.That(viewCount, Does.Match(@"\d+ views"));
    }

    [Test]
    public async Task InvalidArticleSlug_ShowsNotFound()
    {
        // will break if we either add slug or add custom 404 page
        try
        {
            var response = await Page.GotoAsync($"{BaseUrl}/article/non-existent-article");
            Assert.That(response!.Status, Is.EqualTo(404));
        }
        catch (PlaywrightException ex) when (ex.Message.Contains("ERR_HTTP_RESPONSE_CODE_FAILURE"))
        {
            Assert.Pass("404 response confirmed");
        }
    }
}
