namespace Zaabee.RabbitMQ.Demo;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly IZaabeeRabbitMqClient _messageBus;
    private readonly IServiceProvider _serviceProvider;

    public RabbitMqBackgroundService(IZaabeeRabbitMqClient messageBus, IServiceProvider serviceProvider)
    {
        _messageBus = messageBus;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageBus.SubscribeEvent<TestEvent>(() =>
        {
            using var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<Subscriber>().TestEventHandler;
        });
        _messageBus.SubscribeEvent<TestEvent>(() =>
        {
            using var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<Subscriber>().TestEventHandler;
        });
        _messageBus.SubscribeEvent<TestEvent>(() =>
        {
            using var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<Subscriber>().TestEventHandlerAsync;
        });
        // _messageBus.ReceiveCommand<TestEvent>(() => _serviceProvider.GetRequiredService<Subscriber>().TestEventHandler);
        _messageBus.ReceiveCommand<TestEvent>(() =>
        {
            using var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<Subscriber>().TestEventExceptionHandler;
        });
        _messageBus.SubscribeEvent<TestEventWithVersion>(() =>
            {
                using var scope = _serviceProvider.CreateScope();
                return scope.ServiceProvider.GetRequiredService<Subscriber>().TestEventExceptionWithVersionHandler;
            },
            prefetchCount: 20);
        _messageBus.SubscribeEvent<TestEventWithVersion>(() =>
            {
                using var scope = _serviceProvider.CreateScope();
                return scope.ServiceProvider.GetRequiredService<Subscriber>().TestEventExceptionWithVersionHandler;
            },
            "TestQueueName", 20);
        _messageBus.ListenMessage<TestMessage>(() =>
        {
            using var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<Subscriber>().TestMessageHandler;
        });
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _messageBus.Dispose();
        await base.StopAsync(cancellationToken);
    }
}