using SharedLib.DTOs.QueuePublisherDTOs;

namespace SharedLib.QueuePublishier
{
    public interface IQueuePublisher
    {
        Task PublishAsync(NewsLetterQueueDto message, CancellationToken cancellationToken);
    }
}
