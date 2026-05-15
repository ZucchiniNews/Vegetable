using ZucchiniCore.Entities;

namespace Zucchinimvc.Application.Services.QueuePublishier.WelcomeQueue
{
    public interface IWelcomeToNewsLetterPublisher
    {
        Task PublishAsync(NewsLetterQueueMessage message, CancellationToken cancellationToken);
    }
}
