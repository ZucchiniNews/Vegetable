namespace Zucchinimvc.Infrastructure.Config
{
    public class CmsSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public int HomeShownArticles { get; set; } = int.MaxValue;
        public int DaysToShow { get; set; } = -30;


    }

}
