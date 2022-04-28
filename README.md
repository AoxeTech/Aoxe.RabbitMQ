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

In addition we have the following json serializers:

[Zaabee.Jil](https://github.com/PicoHex/Zaabee.Serializers/tree/master/src/Zaabee.MsgPack)

[Zaabee.SystemTextJson](https://github.com/PicoHex/Zaabee.Serializers/tree/master/src/Zaabee.SystemTextJson)

[Zaabee.Utf8Json](https://github.com/PicoHex/Zaabee.Serializers/tree/master/src/Zaabee.Utf8Json)

### Asp.net core

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
        Hosts = new List<string> { "192.168.78.130" },
        UserName = "admin",
        Password = "123",
        Serializer = new NewtonsoftJson.Serializer()
    }));
```

Create a message class named "TestEvent" and version control it with the "MessageVersion" attribute.

```CSharp
public class TestEvent
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
}
```

```csharp

[MessageVersion("3.14")]
public class TestEventWithVersion
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
}
```

### Publish

In Zaabee.RabbitMQ we distinguish the different publishing methods by message type and message sending type as follows:

```csharp
void PublishEvent<T>(T @event);
void PublishEvent<T>(string topic, T @event);
void PublishEvent(string topic, byte[] body);

void SendEvent<T>(T @event);
void SendEvent(string topic, byte[] body);

void PublishMessage<T>(T message);
void PublishMessage<T>(string topic, T message);
void PublishMessage(string topic, byte[] body);

void SendMessage<T>(T message);
void SendMessage(string topic, byte[] body);
```

Also they have corresponding asynchronous versions:

```csharp
Task PublishEventAsync<T>(T @event);
Task PublishEventAsync<T>(string topic, T @event);
Task PublishEventAsync(string topic, byte[] body);

Task SendEventAsync<T>(T message);
Task SendEventAsync(string topic, byte[] body);

Task PublishMessageAsync<T>(T message);
Task PublishMessageAsync<T>(string topic, T message);
Task PublishMessageAsync(string topic, byte[] body);

Task SendMessageAsync<T>(T message);
Task SendMessageAsync(string topic, byte[] body);
```

There are two concepts here, message type and message sending type：

- Message type
  - Message: The "Message" type will not be persisted for throughput and performance purposes, and messages will not be transferred to the dead message queue in the event of a consumption exception. The message's exchange is also Durable to false, so the exchange will be lost after the broker restarts.
  - Event: Messages of event type will be persisted and will be transferred to the corresponding dead message queue in case of consumption exceptions. The Durable of event's exchange is true, so that it is not lost when the broker restarts.
- Message sending type
  - Publish: The message will be posted to the corresponding Topic (which is actually the wrapper for the exchange in RabbitMQ), and if there is no queue binding to the exchange, the message will be discarded.
  - Send: When messages are sent to RabbitMQ, a default queue is created in addition to the corresponding exchange (if there is none), and the exchange and queue will be named after the topic.

If the name of the topic is not specified, it will be automatically named by the type of the message, with the following logic:

```csharp
messageType.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault() is MessageVersionAttribute msgVerAttr
    ? $"{type}[{msgVerAttr.Version}]"
    : type.ToString());
```

### Subscribe

As with publish, there are several different methods of subscribing:

```csharp
void SubscribeEvent<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);
void SubscribeEvent<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);
void SubscribeEvent<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = 10);
void SubscribeEvent<T>(string topic, Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

void ReceiveEvent<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);
void ReceiveEvent<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

void SubscribeMessage<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);
void SubscribeMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);
void SubscribeMessage<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = 10);
void SubscribeMessage<T>(string topic, Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

void ReceiveMessage<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);
void ReceiveMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

void ListenMessage<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);
void ListenMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);
void ListenMessage<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = 10);
void ListenMessage<T>(string topic, Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);
```

- Subscribe: Will automatically create (if not already) a queue named by resolve to bind to the exchange and consume it.
- Receive: As opposed to "Send", will consume messages from the queue created by send.
- Listen: We know that multiple nodes subscribe/receive to the same queue, the messages in this queue will be pushed to these nodes to achieve a balanced load, that is, a single message will only be consumed by a single node in the cluster; while "Listen" allows a single node to have an independent exclusive queue, and automatically delete this queue when the connection is disconnected, usually used in scenarios where all nodes need to be notified.

Also these methods corresponding asynchronous versions too.
