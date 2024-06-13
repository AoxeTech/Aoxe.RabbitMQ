# Aoxe.RabbitMQ

[RabbitMQ](http://www.rabbitmq.com/) is the most widely deployed open source message broker.

With more than 35,000 production deployments of RabbitMQ world-wide at small startups and large enterprises, RabbitMQ is the most popular open source message broker.

RabbitMQ is lightweight and easy to deploy on premises and in the cloud. It supports multiple messaging protocols. RabbitMQ can be deployed in distributed and federated configurations to meet high-scale, high-availability requirements ([GitHub](https://github.com/rabbitmq/rabbitmq-server)).

## QuickStart

### NuGet

```CLI
Install-Package Aoxe.RabbitMQ
Install-Package Aoxe.NewtonsoftJson
```

In addition we have the following json serializers:

[Aoxe.Jil](https://github.com/AoxeTech/Aoxe.Serialization/tree/master/src/Aoxe.MsgPack)

[Aoxe.SystemTextJson](https://github.com/AoxeTech/Aoxe.Serialization/tree/master/src/Aoxe.SystemTextJson)

[Aoxe.Utf8Json](https://github.com/AoxeTech/Aoxe.Serialization/tree/master/src/Aoxe.Utf8Json)

### Asp.net core

Import reference in startup.cs

```CSharp
using Aoxe.RabbitMQ;
using Aoxe.RabbitMQ.Abstractions;
using Aoxe.NewtonsoftJson;
```

Register AoxeRabbitMqClient in ConfigureServices method

```CSharp
services.AddSingleton<IAoxeRabbitMqClient>(_ =>
    new AoxeRabbitMqClient(new AoxeRabbitMqOptions
    {
        AutomaticRecoveryEnabled = true,
        Hosts = new List<string> { `192.168.78.130` },
        UserName = `admin`,
        Password = `123`,
        Serializer = new NewtonsoftJson.Serializer()
    }));
```

Create a message class named `TestEvent` and version control it with the `MessageVersion` attribute.

```CSharp
public class TestEvent
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
}
```

```csharp
[MessageVersion(`3.14`)]
public class TestEventWithVersion
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
}
```

### Publish

In Aoxe.RabbitMQ we distinguish the different publishing methods by message type and message sending type as follows:

```csharp
void PublishEvent<T>(T @event);
void PublishEvent<T>(string topic, T @event);
void PublishEvent(string topic, byte[] body);

void SendCommand<T>(T command);
void SendCommand(string topic, T command);
void SendCommand(string topic, byte[] body);

void PublishMessage<T>(T message);
void PublishMessage<T>(string topic, T message);
void PublishMessage(string topic, byte[] body);
```

There are two concepts here, `message type` and `message sending type`：

- Message type
  - Message: The `Message` type will not be persisted for throughput and performance purposes, and messages will not be transferred to the dead message queue in the event of a consumption exception. The message's exchange is also Durable to false, so the exchange will be lost after the broker restarts.
  - Event: Messages of event type will be persisted and will be transferred to the corresponding dead message queue in case of consumption exceptions. The Durable of event's exchange is true, so that it is not lost when the broker restarts.
  - Command: An `Event` represents something that has already happened, and the publisher does not care whether or not any consumer cares about the event, or what it does with the event. A `command` represents a message that the publisher expects a consumer to process. So `PublishEvent` just posts the event to the exchange, and doesn't create a queue; send command creates a queue with the same name as the exchange / topic, and expects someone to handle it.
- Message sending type
  - Publish: The message will be posted to the corresponding Topic (which is actually the wrapper for the exchange in RabbitMQ), and if there is no queue binding to the exchange, the message will be discarded.
  - Send: When messages are sent to RabbitMQ, a default queue is created in addition to the corresponding exchange (if there is none), and the exchange and queue will be named after the topic.

### Subscribe

As with publish, there are several different methods of subscribing:

```csharp
void SubscribeEvent<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10, int consumeRetry = Consts.DefaultConsumeRetry, bool dlx = true);
void SubscribeEvent<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10, int consumeRetry = Consts.DefaultConsumeRetry, bool dlx = true);
void SubscribeEvent<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = 10, int consumeRetry = Consts.DefaultConsumeRetry, bool dlx = true);
void SubscribeEvent<T>(string topic, Func<Func<T?, Task>> resolve, ushort prefetchCount = 10, int consumeRetry = Consts.DefaultConsumeRetry, bool dlx = true);

void ReceiveCommand<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);
void ReceiveCommand<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);
void ReceiveCommand<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = 10);
void ReceiveCommand<T>(string topic, Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

void ListenMessage<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);
void ListenMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);
void ListenMessage<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = 10);
void ListenMessage<T>(string topic, Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);
```

- Subscribe: Will automatically create (if not already) a queue named by resolve to bind to the exchange and consume it. And it can set the consume retry count, if it still process fail, the `event` will be sent to the dlx queue.
- Receive: As opposed to `Send`, will consume messages from the queue created by send.
- Listen: We know that multiple nodes subscribe / receive to the same queue, the messages in this queue will be pushed to these nodes to achieve a balanced load, that is, a single message will only be consumed by a single node in the cluster; while `Listen` allows a single node to have an independent exclusive queue, and automatically delete this queue when the connection is disconnected, usually used in scenarios where all nodes need to be notified.

Also these methods corresponding asynchronous versions too.
