namespace Zucchinimvc.Application.Services.UsersService.DTOs;

public class NewsletterChangeResultDto
{
    public bool Success { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
    public string StatusType { get; set; } = "success"; // "success", "info", "error"
    public bool WasSubscriptionStateChanged { get; set; }
    public bool? NewSubscriptionState { get; set; }
}
