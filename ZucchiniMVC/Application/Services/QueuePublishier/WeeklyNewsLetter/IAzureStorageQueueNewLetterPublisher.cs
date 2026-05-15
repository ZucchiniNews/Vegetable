using ZucchiniCore.Entities;

namespace Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue
{
    public interface IAzureStorageQueueNewLetterPublisher
    {
        Task PublishAsync(NewsLetterQueueMessage message, CancellationToken cancellationToken);
    }
}

