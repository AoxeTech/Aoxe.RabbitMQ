namespace Aoxe.RabbitMQ.Demo.Controllers;

[Route("api/[controller]/[action]")]
public class RabbitMqDemoController : Controller
{
    private readonly IAoxeRabbitMqClient _messageBus;

    public RabbitMqDemoController(IAoxeRabbitMqClient messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpPost]
    public long PublishEventSync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.PublishEvent(
                new TestEvent { Id = Guid.NewGuid(), Timestamp = DateTimeOffset.Now }
            );
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpPost]
    public long PublishEventWithVersion(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.PublishEvent(
                new TestEventWithVersion { Id = Guid.NewGuid(), Timestamp = DateTimeOffset.Now }
            );
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpPost]
    public long SendCommandSync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.SendCommand(
                new TestEvent { Id = Guid.NewGuid(), Timestamp = DateTimeOffset.Now }
            );
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpPost]
    public long SendCommandWithVersion(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.SendCommand(
                new TestEventWithVersion { Id = Guid.NewGuid(), Timestamp = DateTimeOffset.Now }
            );
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpPost]
    public long PublishMessageSync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.PublishMessage(
                new TestMessage { Id = Guid.NewGuid(), Timestamp = DateTimeOffset.Now }
            );
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpPost]
    public long SendMessageSync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.PublishMessage(
                new TestMessage { Id = Guid.NewGuid(), Timestamp = DateTimeOffset.Now }
            );
        }

        return sw.ElapsedMilliseconds;
    }
}
