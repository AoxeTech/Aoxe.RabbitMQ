namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void ReceiveMessage<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        Subscribe(topic, topic, resolve, MessageType.Message, prefetchCount);
    }

    public void ReceiveMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        Subscribe(topic, topic, resolve, MessageType.Message, prefetchCount);
    }

    public void SubscribeMessage<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        Subscribe(topic, queue, resolve, MessageType.Message, prefetchCount);
    }

    public void SubscribeMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        Subscribe(topic, queue, resolve, MessageType.Message, prefetchCount);
    }

    public void SubscribeMessage<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        Subscribe(topic, queue, resolve, MessageType.Message, prefetchCount);
    }

    public void SubscribeMessage<T>(string topic, Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        Subscribe(topic, queue, resolve, MessageType.Message, prefetchCount);
    }

    public void ListenMessage<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        Subscribe(topic, queue, resolve, MessageType.Message, prefetchCount);
    }

    public void ListenMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        Subscribe(topic, queue, resolve, MessageType.Message, prefetchCount);
    }
}