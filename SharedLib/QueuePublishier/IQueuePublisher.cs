using SharedLib.QueuePublishier.DTOs;

namespace SharedLib.QueuePublishier
{
    public interface IQueuePublisher
    {
        Task PublishAsync(NewsLetterQueueDto message, CancellationToken cancellationToken);
    }
}
