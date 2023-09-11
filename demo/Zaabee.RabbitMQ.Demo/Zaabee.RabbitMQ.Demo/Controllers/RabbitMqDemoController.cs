namespace Zaabee.RabbitMQ.Demo.Controllers;

[Route("api/[controller]/[action]")]
public class RabbitMqDemoController : Controller
{
    private readonly IZaabeeRabbitMqClient _messageBus;

    public RabbitMqDemoController(IZaabeeRabbitMqClient messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpGet]
    [HttpPost]
    public long PublishEventSync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.PublishEvent(new TestEvent
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            });
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpGet]
    [HttpPost]
    public async Task<long> PublishEventAsync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            await _messageBus.PublishEventAsync(new TestEvent
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            });
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpGet]
    [HttpPost]
    public long PublishEventWithVersion(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.PublishEvent(new TestEventWithVersion
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            });
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpGet]
    [HttpPost]
    public long SendCommandSync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.SendCommand(new TestEvent
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            });
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpGet]
    [HttpPost]
    public async Task<long> SendCommandAsync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            await _messageBus.SendCommandAsync(new TestEvent
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            });
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpGet]
    [HttpPost]
    public long SendCommandWithVersion(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.SendCommand(new TestEventWithVersion
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            });
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpGet]
    [HttpPost]
    public long PublishMessageSync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.Publish(new TestMessage
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            }, false);
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpGet]
    [HttpPost]
    public async Task<long> PublishMessageAsync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            await _messageBus.PublishAsync(new TestMessage
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            }, false);
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpGet]
    [HttpPost]
    public long SendMessageSync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.Send(new TestMessage
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            }, false);
        }

        return sw.ElapsedMilliseconds;
    }

    [HttpGet]
    [HttpPost]
    public async Task<long> SendMessageAsync(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            await _messageBus.SendAsync(new TestMessage
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            }, false);
        }

        return sw.ElapsedMilliseconds;
    }
}