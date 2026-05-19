using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace ZucchiniMVC.E2ETests;

public class LikeTests : ZucchiniPageTest
{
    private const string ArticleUrl = "/Article/free-article";
    private const string TestEmail = "testuser@zucchinews.com";
    private const string TestPassword = "TestPassword123!";

    private async Task Login()
    {
        await Page.GotoAsync($"{BaseUrl}/Identity/Account/Login");
        await Page.Locator("#Input_Email").FillAsync(TestEmail);
        await Page.Locator("#Input_Password").FillAsync(TestPassword);
        await Page.ClickAsync("button[type='submit']");
    }
    
    private async Task<int> GetLikeCount()
    {
        var likeCountText = await Page.Locator("#likeCount").InnerTextAsync();
        var match = Regex.Match(likeCountText, @"\d+");
        return match.Success ? int.Parse(match.Value) : 0;
    }

    private async Task EnsureNotLiked()
    {
        var btnClass = await Page.Locator("#like-btn").GetAttributeAsync("class");
        if (btnClass != null && btnClass.Contains("btn-danger"))
        {
            await Page.Locator("#like-btn").ClickAsync();
            await Page.WaitForTimeoutAsync(500); // Wait for the like count to update
        }
    }

    [Test]
    [Category("WriteOnly")]
    public async Task AuthenticatedUser_LikeButton_IncrementsLikeCount()
    {
        await Login();
        await Page.GotoAsync($"{BaseUrl}{ArticleUrl}");

        var initialCount = await GetLikeCount();
        await Page.Locator("#like-btn").ClickAsync();
        await Page.WaitForTimeoutAsync(500); // Wait for the like count to update
        var updatedCount = await GetLikeCount();

        Assert.That(updatedCount, Is.EqualTo(initialCount + 1));
    }

    [Test]
    [Category("WriteOnly")]
    public async Task LikingSameArticleTwice_DoesNotIncrementLikeCount()
    {
        await Login();
        await Page.GotoAsync($"{BaseUrl}{ArticleUrl}");
        await EnsureNotLiked();

        await Page.Locator("#like-btn").ClickAsync();
        await Page.WaitForTimeoutAsync(500); // Wait for the like count to update
        var afterFirstLike = await GetLikeCount();

        await Page.Locator("#like-btn").ClickAsync();
        await Page.WaitForTimeoutAsync(500); // Wait for the like count to update
        var afterSecondLike = await GetLikeCount();

        Assert.That(afterSecondLike, Is.EqualTo(afterFirstLike - 1));
    }

    [Test]
    [Category("ReadOnly")]
    public async Task UnauthenticatedUser_LikeButton_IsPromptedToLogin()
    {
        await Page.GotoAsync($"{BaseUrl}{ArticleUrl}");

        await Page.Locator("button[onclick='showLoginPrompt()']").ClickAsync();

        await Expect(Page.Locator("#loginPromptModal")).ToBeVisibleAsync();
    }

    [Test]
    [Category("ReadOnly")]
    public async Task LikeCount_PersistsAfterPageReload()
    {
        await Login();
        await Page.GotoAsync($"{BaseUrl}{ArticleUrl}");
        await EnsureNotLiked();

        var initialCount = await GetLikeCount();
        await Page.Locator("#like-btn").ClickAsync();
        await Page.WaitForTimeoutAsync(500); // Wait for the like count to update

        await Page.ReloadAsync();

        var updatedCount = await GetLikeCount();

        Assert.That(updatedCount, Is.EqualTo(initialCount + 1), "Like count should persist after page reload.");
    }
}
