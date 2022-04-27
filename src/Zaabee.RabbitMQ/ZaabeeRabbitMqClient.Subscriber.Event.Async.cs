namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task ReceiveEventAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        await SubscribeAsync(topic, topic, resolve, MessageType.Event, prefetchCount);
    }

    public async Task ReceiveEventAsync<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        await SubscribeAsync(topic, topic, resolve, MessageType.Event, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        await SubscribeAsync(topic, queue, resolve, MessageType.Event, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        await SubscribeAsync(topic, queue, resolve, MessageType.Event, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(string topic, Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        await SubscribeAsync(topic, queue, resolve, MessageType.Event, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(string topic, Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        await SubscribeAsync(topic, queue, resolve, MessageType.Event, prefetchCount);
    }
}