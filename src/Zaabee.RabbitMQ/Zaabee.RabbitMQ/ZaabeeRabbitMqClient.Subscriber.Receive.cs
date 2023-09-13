namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void Receive<T>(
        Func<Action<T?>> resolve,
        bool persistence,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence);
        Consume(
            normalExchangeParam,
            normalQueueParam,
            null,
            null,
            resolve,
            prefetchCount,
            0);
    }

    public void Receive<T>(
        Func<Func<T?, Task>> resolve,
        bool persistence,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence);
        Consume(
            normalExchangeParam,
            normalQueueParam,
            null,
            null,
            resolve,
            prefetchCount,
            0);
    }

    public void Receive<T>(
        string topic,
        Func<Action<T?>> resolve,
        bool persistence,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence);
        Consume(
            normalExchangeParam,
            normalQueueParam,
            null,
            null,
            resolve,
            prefetchCount,
            0);
    }

    public void Receive<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        bool persistence,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence);
        Consume(
            normalExchangeParam,
            normalQueueParam,
            null,
            null,
            resolve,
            prefetchCount,
            0);
    }

    public void Receive(
        string topic,
        Func<Action<byte[]>> resolve,
        bool persistence,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence);
        Consume(
            normalExchangeParam,
            normalQueueParam,
            null,
            null,
            resolve,
            prefetchCount,
            0);
    }

    public void Receive(
        string topic,
        Func<Func<byte[], Task>> resolve,
        bool persistence,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence);
        Consume(
            normalExchangeParam,
            normalQueueParam,
            null,
            null,
            resolve,
            prefetchCount,
            0);
    }
}