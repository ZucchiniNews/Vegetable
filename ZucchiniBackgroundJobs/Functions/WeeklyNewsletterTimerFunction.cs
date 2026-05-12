using Microsoft.Azure.Functions.Worker;
using Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue;


namespace ZucchiniBackgroundJobs.Functions
{
    public class WeeklyNewsletterTimerFunction
    {
        private readonly WeeklyNewsLetterOrchestrator _orchestrator;

        public WeeklyNewsletterTimerFunction(
            WeeklyNewsLetterOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [Function("WeeklyNewsletterTimer")]
        public async Task Run(
            [TimerTrigger("0 0 8 * * MON")]
        TimerInfo timer,
            CancellationToken cancellationToken)
        {
            await _orchestrator.PublishWeeklyNewsLetterAsync(cancellationToken);
        }

    }
}
