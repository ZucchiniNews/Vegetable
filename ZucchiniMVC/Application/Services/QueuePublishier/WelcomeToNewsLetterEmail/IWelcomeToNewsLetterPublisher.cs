using zucchiniMVC.Models.DTOs.QueuePublisherDOTs;

namespace Zucchinimvc.Application.Services.QueuePublishier.WelcomeToNewsLetterPublisher
{
    public interface IWelcomeToNewsLetterPublisher
    {
        Task PublishAsync(NewsLetterQueueDto message, CancellationToken cancellationToken);
    }
}
