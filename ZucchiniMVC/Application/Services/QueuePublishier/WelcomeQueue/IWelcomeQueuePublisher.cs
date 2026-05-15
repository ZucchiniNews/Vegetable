using ZucchiniCore.Entities;

namespace Zucchinimvc.Application.Services.QueuePublishier.WelcomeQueue
{
    public interface IWelcomeQueuePublisher
    {
        Task PublishAsync(NewsLetterQueueMessage message, CancellationToken cancellationToken);
    }
}
