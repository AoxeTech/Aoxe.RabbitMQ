namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void SubscribeEvent<T>(
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(topic, MessageType.Event, SubscribeType.Receive);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public void SubscribeEvent<T>(
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(topic, MessageType.Event, SubscribeType.Receive);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public void SubscribeEvent<T>(
        string topic,
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public void SubscribeEvent<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public void SubscribeEvent<T>(
        string topic,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public void SubscribeEvent<T>(
        string topic,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }
}