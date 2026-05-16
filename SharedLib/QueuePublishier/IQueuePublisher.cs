
using SharedLib.DTOs.QueuePublisherDOTs;

namespace SharedLib.QueuePublishier
{
    public interface IQueuePublisher
    {
        Task PublishAsync(NewsLetterQueueDto message, CancellationToken cancellationToken);
    }
}
