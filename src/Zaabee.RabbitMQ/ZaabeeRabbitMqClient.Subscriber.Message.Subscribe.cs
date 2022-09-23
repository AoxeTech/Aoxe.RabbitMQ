namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void SubscribeMessage<T>(
        Func<Action<T?>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public void SubscribeMessage<T>(
        Func<Func<T?, Task>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public void SubscribeMessage<T>(
        string topic,
        Func<Action<T?>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public void SubscribeMessage<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public void SubscribeMessage<T>(
        string topic,
        Func<Action<byte[]>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public void SubscribeMessage<T>(
        string topic,
        Func<Func<byte[], Task>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Subscribe);
        Subscribe(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }
}