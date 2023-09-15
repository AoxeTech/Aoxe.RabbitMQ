namespace Zaabee.RabbitMQ.Demo;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly IZaabeeRabbitMqClient _messageBus;

    public RabbitMqBackgroundService(IZaabeeRabbitMqClient messageBus)
    {
        _messageBus = messageBus;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageBus.SubscribeEvent<TestEvent>(() => new Subscriber().TestEventHandler);
        _messageBus.SubscribeEvent<TestEvent>(() => new Subscriber().TestEventHandlerAsync);
        // _messageBus.ReceiveCommand<TestEvent>(() => new Subscriber().TestEventHandler);
        _messageBus.ReceiveCommand<TestEvent>(() => new Subscriber().TestEventExceptionHandler);
        _messageBus.SubscribeEvent<TestEventWithVersion>(() => new Subscriber().TestEventExceptionWithVersionHandler, 20);
        _messageBus.SubscribeMessage<TestMessage>(() => new Subscriber().TestMessageHandler);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _messageBus.Dispose();
        await base.StopAsync(cancellationToken);
    }
}