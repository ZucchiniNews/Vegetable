using System.Text.RegularExpressions;

namespace Zucchinimvc.Application.Services.Articles;

public class UtilsService : IUtilsService
{
    private const int WordsPerMinute = 225;
    public int CalculateReadTime(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return 0;

        var plainText = Regex.Replace(content, "<[^>]*>", "");

        var wordCount = plainText.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

        return (int)Math.Ceiling((double)wordCount / WordsPerMinute);
    }
}
