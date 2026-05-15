using ZucchiniCore.Entities;

namespace Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue
{
    public interface IWeeklyNewsLetterPublisher
    {
        Task PublishAsync(NewsLetterQueueMessage message, CancellationToken cancellationToken);
    }
}

