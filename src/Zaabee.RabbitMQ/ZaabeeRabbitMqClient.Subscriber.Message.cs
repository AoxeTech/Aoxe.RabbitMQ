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

        var exchangeParam = new ExchangeParam { Exchange = topic, Durable = false };
        var queueParam = new QueueParam { Queue = queue, Durable = false, Exclusive = true, AutoDelete = true };
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

        ConsumeMessage(channel, resolve, queueParam.Queue);
    }

    public void ListenMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";

        var exchangeParam = new ExchangeParam { Exchange = topic, Durable = false };
        var queueParam = new QueueParam { Queue = queue, Durable = false, Exclusive = true, AutoDelete = true };
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

        ConsumeMessage(channel, resolve, queueParam.Queue);
    }

    public void ListenMessage<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = 10)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";

        var exchangeParam = new ExchangeParam { Exchange = topic, Durable = false };
        var queueParam = new QueueParam { Queue = queue, Durable = false, Exclusive = true, AutoDelete = true };
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

        ConsumeMessage(channel, resolve, queueParam.Queue);
    }

    public void ListenMessage<T>(string topic, Func<Func<T?, Task>> resolve, ushort prefetchCount = 10)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";

        var exchangeParam = new ExchangeParam { Exchange = topic, Durable = false };
        var queueParam = new QueueParam { Queue = queue, Durable = false, Exclusive = true, AutoDelete = true };
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

        ConsumeMessage(channel, resolve, queueParam.Queue);
    }
}