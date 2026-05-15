using ZucchiniCore.Entities;

namespace Zucchinimvc.Application.Services.QueuePublishie.WelcomeToNewsLetterEmail
{
    public interface IWeeklyNewsLetterPublisher
    {
        Task PublishAsync(NewsLetterQueueMessage message, CancellationToken cancellationToken);
    }
}

