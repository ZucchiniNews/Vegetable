using zucchini_functions.WeeklyNewsLetterEmail.DTOs;

namespace zucchini_functions.WeeklyNewsLetterEmail
{
    public interface IWeeklyNewsLetter
    {
        Task PublishAsync(NewsLetterQueueDto message, CancellationToken cancellationToken);
    }

}

