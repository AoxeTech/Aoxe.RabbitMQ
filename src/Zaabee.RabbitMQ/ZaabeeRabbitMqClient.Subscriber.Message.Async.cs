namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task ReceiveMessageAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        await SubscribeAsync(topic, topic, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ReceiveMessageAsync<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        await SubscribeAsync(topic, topic, resolve, MessageType.Message, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        await SubscribeAsync(topic, queue, resolve, MessageType.Message, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        await SubscribeAsync(topic, queue, resolve, MessageType.Message, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(string topic, Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        await SubscribeAsync(topic, queue, resolve, MessageType.Message, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(string topic, Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        await SubscribeAsync(topic, queue, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        await SubscribeAsync(topic, queue, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync<T>(Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        await SubscribeAsync(topic, queue, resolve, MessageType.Message, prefetchCount);
    }
}