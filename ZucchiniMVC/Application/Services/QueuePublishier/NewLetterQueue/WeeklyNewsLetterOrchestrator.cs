using Zucchinimvc.Application.Services.Users;

namespace Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue
{
    public class WeeklyNewsLetterOrchestrator
    {
        private readonly IUserService _userService;
        private readonly INewsLetterQueuePublisher _newsLetterQueuePublisher;

        public WeeklyNewsLetterOrchestrator(IUserService userService, INewsLetterQueuePublisher newsLetterQueuePublisher)
        {
            _userService = userService;
            _newsLetterQueuePublisher = newsLetterQueuePublisher;
        }

        public async Task PublishWeeklyNewsLetterAsync(CancellationToken cancellationToken)
        {
            var users = await _userService.GetNewsletterSubscribersAsync().ConfigureAwait(false);
            foreach (var user in users)
            {
                var message = new NewsLetterQueueMessage
                {
                    Email = user.Email,
                    Subject = "Weekly Newsletter",
                    HtmlBody = "<h1>Welcome to our Weekly Newsletter!</h1><p>Here are the latest updates...</p>"
                };
                await _newsLetterQueuePublisher.PublishAsync(message, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
