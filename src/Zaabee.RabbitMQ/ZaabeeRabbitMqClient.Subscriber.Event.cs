namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void ReceiveEvent<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var eventName = GetTypeName(typeof(T));
        SubscribeEvent(eventName, eventName, resolve, prefetchCount);
    }

    public void ReceiveEvent<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var eventName = GetTypeName(typeof(T));
        SubscribeEvent(eventName, eventName, resolve, prefetchCount);
    }

    public void SubscribeEvent<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var eventName = GetTypeName(typeof(T));
        SubscribeEvent(eventName, resolve, prefetchCount);
    }

    public void SubscribeEvent<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var eventName = GetTypeName(typeof(T));
        SubscribeEvent(eventName, resolve, prefetchCount);
    }

    public void SubscribeEvent<T>(string exchange, Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        SubscribeEvent(exchange, queue, resolve, prefetchCount);
    }

    public void SubscribeEvent<T>(string exchange, Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        SubscribeEvent(exchange, queue, resolve, prefetchCount);
    }

    public void SubscribeEvent<T>(string exchange, string queue, Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var exchangeParam = new ExchangeParam { Exchange = exchange };
        var queueParam = new QueueParam { Queue = queue };
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

        ConsumeEvent(channel, resolve, queueParam.Queue);
    }

    public void SubscribeEvent<T>(string exchange, string queue, Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var exchangeParam = new ExchangeParam { Exchange = exchange };
        var queueParam = new QueueParam { Queue = queue };
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

        ConsumeEvent(channel, resolve, queueParam.Queue);
    }
}