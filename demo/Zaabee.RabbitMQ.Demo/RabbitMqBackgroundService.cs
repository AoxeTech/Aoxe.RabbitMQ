using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Zaabee.RabbitMQ.Abstractions;

namespace Zaabee.RabbitMQ.Demo
{
    public class RabbitMqBackgroundService : BackgroundService
    {
        private readonly IZaabeeRabbitMqClient _messageBus;

        public RabbitMqBackgroundService(IZaabeeRabbitMqClient messageBus)
        {
            _messageBus = messageBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageBus.SubscribeEventAsync(() => new Subscriber().TestEventHandler);
            await _messageBus.SubscribeEventAsync(() => new Subscriber().TestEventHandler, 30);
            await _messageBus.SubscribeEventAsync(
                () => new Subscriber().TestEventExceptionWithVersionHandler, 20);
            await _messageBus.SubscribeMessageAsync(() => new Subscriber().TestMessageHandler);
            _messageBus.RepublishDeadLetterEvent<TestEvent>(
                "dead-letter-Zaabee.RabbitMQ.Demo.Subscriber.TestEventExceptionHandler[Zaabee.RabbitMQ.Demo.TestEvent]");
            _messageBus.RepublishDeadLetterEvent<TestEvent>(
                "dead-letter-Zaabee.RabbitMQ.Demo.TestEvent");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _messageBus.Dispose();
            await base.StopAsync(cancellationToken);
        }
    }
}