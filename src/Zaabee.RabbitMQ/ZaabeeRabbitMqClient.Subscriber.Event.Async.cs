namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task ReceiveEventAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(topic, MessageType.Event, SubscribeType.Receive);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public async Task ReceiveEventAsync<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(topic, MessageType.Event, SubscribeType.Receive);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(string topic, Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(string topic, Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }
}