namespace Zucchinimvc.Models.ViewModels;

public class NewsletterSubscriptionToggleViewModel
{
    public bool IsSubscribed { get; set; }
    public string Email { get; set; } = string.Empty;
}
