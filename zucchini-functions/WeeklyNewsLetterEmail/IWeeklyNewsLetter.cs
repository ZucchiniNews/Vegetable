using ZucchiniCore.Entities;

namespace zucchini_functions.WeeklyNewsLetterEmail
{
    public interface IWeeklyNewsLetter
    {
        Task PublishAsync(NewsLetterQueueMessage message, CancellationToken cancellationToken);
    }

}

