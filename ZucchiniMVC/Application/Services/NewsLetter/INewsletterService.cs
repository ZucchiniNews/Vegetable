
namespace ZucchiniMVC.Application.Services.NewsLetter
{
    public interface INewsLetterService
    {
        Task SendNewsLetterEmailAsync(string email, string subject, string content, CancellationToken cancellationToken);
    }
}