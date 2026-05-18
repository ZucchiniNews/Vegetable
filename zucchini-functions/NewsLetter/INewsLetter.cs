namespace zucchini_functions.NewsLetter
{
    public interface INewsLetter
    {
        Task SendEmail(string message, CancellationToken cancellationToken);
    }

}

