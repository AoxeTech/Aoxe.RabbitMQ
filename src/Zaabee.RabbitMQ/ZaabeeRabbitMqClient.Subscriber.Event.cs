namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void ReceiveEvent<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(topic, MessageType.Event, SubscribeType.Receive);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public void ReceiveEvent<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(topic, MessageType.Event, SubscribeType.Receive);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public void SubscribeEvent<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public void SubscribeEvent<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public void SubscribeEvent<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }

    public void SubscribeEvent<T>(string topic, Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(queue, MessageType.Event, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Event, prefetchCount);
    }
}