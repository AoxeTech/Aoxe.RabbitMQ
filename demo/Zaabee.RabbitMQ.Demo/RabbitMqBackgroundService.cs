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
            _messageBus.ReceiveEvent<TestEvent>(new Subscriber().TestEventHandler);
            _messageBus.SubscribeEvent<TestEvent>(new Subscriber().TestEventHandler);

            _messageBus.SubscribeEvent<TestEvent>(() => new Subscriber().TestEventHandler);
            _messageBus.SubscribeEvent<TestEvent>(() => new Subscriber().TestEventHandler, 30);
            _messageBus.ReceiveEvent<TestEvent>(new Subscriber().TestEventExceptionHandler);
            _messageBus.SubscribeEvent<TestEvent>(new Subscriber().TestEventExceptionHandler);
            _messageBus.ReceiveEvent<TestEventWithVersion>(new Subscriber().TestEventWithVersionHandler);
            _messageBus.SubscribeEvent<TestEventWithVersion>(
                () => new Subscriber().TestEventExceptionWithVersionHandler, 20);
            _messageBus.ReceiveMessage<TestMessage>(new Subscriber().TestMessageHandler);
            _messageBus.SubscribeMessage<TestMessage>(new Subscriber().TestMessageHandler);
            _messageBus.SubscribeMessage<TestMessage>(() => new Subscriber().TestMessageHandler);
            _messageBus.ListenMessage<TestMessage>(new Subscriber().TestMessageHandler);
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