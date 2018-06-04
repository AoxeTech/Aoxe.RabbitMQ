# Zaabee.RabbitMQ

[RabbitMQ](http://www.rabbitmq.com/) is the most widely deployed open source message broker.

With more than 35,000 production deployments of RabbitMQ world-wide at small startups and large enterprises, RabbitMQ is the most popular open source message broker.

RabbitMQ is lightweight and easy to deploy on premises and in the cloud. It supports multiple messaging protocols. RabbitMQ can be deployed in distributed and federated configurations to meet high-scale, high-availability requirements ([GitHub](https://github.com/rabbitmq/rabbitmq-server)).

## QuickStart

### NuGet

    Install-Package Zaaby.RabbitMQ
    Install-Package Zaaby.RabbitMQ.Json

### Asp.net core

#### Build Project

Import reference in startup.cs

```CSharp
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Core;
using Zaabee.RabbitMQ.ISerialize;
using Zaabee.RabbitMQ.Json;
```

Register ZabbyRabbitMqClient in ConfigureServices method

```CSharp
services.AddSingleton<ISerializer, Serializer>();
services.AddSingleton<IMessageBus, ZaabyRabbitMqClient>(p =>
    new ZaabyRabbitMqClient(new MqConfig
    {
        AutomaticRecoveryEnabled = true,
        HeartBeat = 60,
        NetworkRecoveryInterval = new TimeSpan(60),
        Hosts = new List<string>{"192.168.78.152"},
        UserName = "admin",
        Password = "admin"
    }, services.BuildServiceProvider().GetService<ISerializer>()));
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
    private readonly IMessageBus _messageBus;

    public RabbitMqDemoController(IMessageBus messageBus)
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
public class ServiceRunner
{
    private readonly IEventBus _mqHandler;

    public ServiceRunner(IEventBus handler)
    {
        _mqHandler = handler;
    }

    public void Start()
    {
        _mqHandler.ReceiveEvent<TestEvent>(TestEventHandler);
        _mqHandler.ReceiveEvent<TestEvent>(TestEventExceptionHandler);
        _mqHandler.ReceiveEvent<TestEventWithVersion>(TestEventWithVersionHandler);
        _mqHandler.ReceiveEvent<TestEventWithVersion>(TestEventExceptionWithVersionHandler);
        _mqHandler.ReceiveMessage<TestMessage>(TestMessageHandler);
    }

    public void TestEventHandler(TestEvent testEvent)
    {

    }

    public void TestEventExceptionHandler(TestEvent testEvent)
    {
        throw new Exception("Test");
    }

    public void TestEventWithVersionHandler(TestEventWithVersion testEventWithVersion)
    {

    }

    public void TestEventExceptionWithVersionHandler(TestEventWithVersion testEventWithVersion)
    {
        throw new Exception("Test");
    }

    public void TestMessageHandler(TestMessage testMessage)
    {

    }
}
```

Register it in the ConfigureServices method(Start.cs)

```CSharp
services.AddSingleton<ServiceRunner>();
```

Modify the Configure method like this

```CSharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ServiceRunner runner)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseMvc();
    runner.Start();
}
```

Debug the webapi project you can see the message in the queues will be subscribed.And some of them will be republished to the dead letter queues.

### Notion

The IEvent has two subscribe types and IMessage has three

    ReceiveEvent
    SubscribeEvent

    ReceiveMessage
    SubscribeMessage
    ListenMessage

The differences between IEvent and IMessage is that IEvent will persist messages but IMessage will not.IMessage is designed for performance,thus it will not persist messages in the exchange and queue.

When you send a message at first time it will create default exchange and queue named by the message full class name.The RECEIVE method will get the message from the default queue.The SUBSCRIBE method will create a new queue named by the handle and binding it to the message default exchange.So when you want to extend your service logic you just need to subscribe it and the previous services didn't need to recode or release.

The LISTEN method based by the exclusive queue.It is for each node but not the cluster.When you need to refresh the local cache or the config you can use it.When the connection close,the LISTEN queue will be deleted.
