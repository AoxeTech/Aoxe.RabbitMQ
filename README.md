# Zaabee.RabbitMQ

[RabbitMQ](http://www.rabbitmq.com/) is the most widely deployed open source message broker.

With more than 35,000 production deployments of RabbitMQ world-wide at small startups and large enterprises, RabbitMQ is the most popular open source message broker.

RabbitMQ is lightweight and easy to deploy on premises and in the cloud. It supports multiple messaging protocols. RabbitMQ can be deployed in distributed and federated configurations to meet high-scale, high-availability requirements ([GitHub](https://github.com/rabbitmq/rabbitmq-server)).

## QuickStart

### NuGet

```CLI
Install-Package Zaabee.RabbitMQ
Install-Package Zaabee.NewtonsoftJson
```

Otherwise we have these serializers:

[Zaabee.Jil](https://github.com/Mutuduxf/Zaabee.Serializers/tree/master/src/Zaabee.MsgPack)

[Zaabee.NewtonsoftJson](https://github.com/Mutuduxf/Zaabee.Serializers/tree/master/src/Zaabee.NewtonsoftJson)

[Zaabee.SystemTextJson](https://github.com/Mutuduxf/Zaabee.Serializers/tree/master/src/Zaabee.SystemTextJson)

[Zaabee.Utf8Json](https://github.com/Mutuduxf/Zaabee.Serializers/tree/master/src/Zaabee.Utf8Json)

[Zaabee.Xml](https://github.com/Mutuduxf/Zaabee.Serializers/tree/master/src/Zaabee.Xml)

### Asp.net core

#### Build Project

Import reference in startup.cs

```CSharp
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.NewtonsoftJson;
```

Register ZabbyRabbitMqClient in ConfigureServices method

```CSharp
services.AddSingleton<IZaabeeRabbitMqClient>(_ =>
    new ZaabeeRabbitMqClient(new ZaabeeRabbitMqOptions
    {
        AutomaticRecoveryEnabled = true,
        HeartBeat = TimeSpan.FromMinutes(1),
        NetworkRecoveryInterval = new TimeSpan(60),
        Hosts = new List<string> { "192.168.78.150" },
        UserName = "admin",
        Password = "123",
        Serializer = new Serializer()
    }));
```

Create classes that implementate the IEvent or IMessage.IEvent means the message will be persisted both in exchange and queue for the RabbitMQ.When the handle throw exception it will be republished to the dead letter queue.

IMessage is implemented for performance,it will not persist the exchange and queue.

```CSharp
public class TestEvent
{
    public Guid Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}

[MessageVersion("3.14")]
public class TestEventWithVersion
{
    public Guid Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}

public class TestMessage
{
    public Guid Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
```

#### Publish

Now add a controller in the webapi project like this

```CSharp
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
    public long PublishEvent(int quantity)
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
    public long PublishMessage(int quantity)
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < quantity; i++)
        {
            _messageBus.PublishMessage(new TestMessage
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now
            });
        }

        return sw.ElapsedMilliseconds;
    }
}
```

You can send request to these actions and the queues will show in the Rabbitmq Management

#### Subscribe

Create a class named ServiceRunner.cs

```CSharp
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
            _messageBus.ReceiveEvent<TestEvent>(TestEventHandler);
            _messageBus.SubscribeEvent<TestEvent>(new Subscriber().TestEventHandler);

            _messageBus.SubscribeEvent<TestEvent>(new Subscriber().TestEventHandler);
            _messageBus.SubscribeEvent<TestEvent>(() => new Subscriber().TestEventHandler, 30);
            _messageBus.ReceiveEvent<TestEvent>(TestEventExceptionHandler);
            _messageBus.SubscribeEvent<TestEvent>(TestEventExceptionHandler);
            _messageBus.ReceiveEvent<TestEventWithVersion>(TestEventWithVersionHandler);
            _messageBus.ReceiveEvent<TestEventWithVersion>(TestEventExceptionWithVersionHandler, 20);
            _messageBus.ReceiveMessage<TestMessage>(TestMessageHandler);
            _messageBus.SubscribeMessage<TestMessage>(new Subscriber().TestMessageHandler);
            _messageBus.SubscribeMessage<TestMessage>(() => new Subscriber().TestMessageHandler);
            _messageBus.ListenMessage<TestMessage>(TestMessageHandler);
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
```

Add the host service in the ConfigureServices method(Start.cs)

```CSharp
services.AddHostedService<RabbitMqBackgroundService>();
```

Debug the webapi project you can see the message in the queues will be subscribed.And some of them will be republished to the dead letter queues.

### Notion

The IEvent has two subscribe types and IMessage has three

* ReceiveEvent
* SubscribeEvent

* ReceiveMessage
* SubscribeMessage
* ListenMessage

The differences between IEvent and IMessage is that IEvent will persist messages but IMessage will not.IMessage is designed for performance,thus it will not persist messages in the exchange and queue.

When you send a message at first time it will create default exchange named by the message full class name.The RECEIVE method will get the message from the queue whitch with the same name as exchange.The SUBSCRIBE method will create a new queue named by the handle and binding it to the message default exchange.So when you want to extend your service logic you just need to subscribe it and the previous services didn't need to recode or release.

The LISTEN method based by the exclusive queue.It is for each node but not the cluster.When you need to refresh the local cache or the config you can use it.When the connection close,the LISTEN queue will be deleted.
