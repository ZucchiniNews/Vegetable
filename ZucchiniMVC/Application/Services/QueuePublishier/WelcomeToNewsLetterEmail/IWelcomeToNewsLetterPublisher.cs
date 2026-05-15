using ZucchiniCore.Entities;

namespace Zucchinimvc.Application.Services.QueuePublishier.WelcomeToNewsLetterPublisher
{
    public interface IWelcomeToNewsLetterPublisher
    {
        Task PublishAsync(NewsLetterQueueMessage message, CancellationToken cancellationToken);
    }
}
