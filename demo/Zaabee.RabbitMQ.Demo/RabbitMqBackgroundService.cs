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
            // _messageBus.ReceiveEvent<TestEvent>(TestEventHandler);
            _messageBus.SubscribeEvent<TestEvent>(new Subscriber().TestEventHandler);

            _messageBus.SubscribeEvent<TestEvent>(new Subscriber().TestEventHandler);
            _messageBus.SubscribeEvent<TestEvent>(() => new Subscriber().TestEventHandler, 30);
            // _messageBus.ReceiveEvent<TestEvent>(TestEventExceptionHandler);
            // _messageBus.SubscribeEvent<TestEvent>(TestEventExceptionHandler);
            // _messageBus.ReceiveEvent<TestEventWithVersion>(TestEventWithVersionHandler);
            // _messageBus.ReceiveEvent<TestEventWithVersion>(TestEventExceptionWithVersionHandler, 20);
            // _messageBus.ReceiveMessage<TestMessage>(TestMessageHandler);
            _messageBus.SubscribeMessage<TestMessage>(new Subscriber().TestMessageHandler);
            _messageBus.SubscribeMessage<TestMessage>(() => new Subscriber().TestMessageHandler);
            // _messageBus.ListenMessage<TestMessage>(TestMessageHandler);
            _messageBus.RepublishDeadLetterEvent<TestEvent>(
                "dead-letter-EmailApplication.EmailEventHandler.Handle[EmailContract.EmailCommand]");
            _messageBus.RepublishDeadLetterEvent<TestEvent>(
                "dead-letter-Demo.TestEvent");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _messageBus.Dispose();
        }
    }
}