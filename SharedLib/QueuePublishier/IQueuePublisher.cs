
using SharedLib.DTOs.QueuePublisherDOTs;

namespace SharedLib.QueuePublisher
{
    public interface IQueuePublisher
    {
        Task PublishAsync(NewsLetterQueueDto message, CancellationToken cancellationToken);
    }
}
