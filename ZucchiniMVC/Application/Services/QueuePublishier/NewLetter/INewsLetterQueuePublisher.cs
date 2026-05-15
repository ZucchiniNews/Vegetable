using ZucchiniCore.Entities;

namespace Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue
{
    public interface INewsLetterQueuePublisher
    {
        Task PublishAsync(NewsLetterQueueMessage message, CancellationToken cancellationToken);
    }
}

