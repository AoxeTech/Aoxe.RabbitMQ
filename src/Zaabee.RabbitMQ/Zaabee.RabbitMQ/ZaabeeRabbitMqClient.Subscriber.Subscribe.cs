namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void Subscribe<T>(Func<Action<T?>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = 3,
        bool dlx = false,
        bool isExclusive = false)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(queue, persistence, isExclusive);
        if (isExclusive)
        {
            Consume(
                normalExchangeParam,
                normalQueueParam,
                null,
                null,
                resolve,
                prefetchCount,
                consumeRetry);
        }
        var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, true) : null;
        var dlxQueueParam = dlx ? GetQueueParam(queue, persistence, isExclusive, true) : null;
        Consume(
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam,
            dlxQueueParam,
            resolve,
            prefetchCount,
            consumeRetry);
    }

    public void Subscribe<T>(Func<Func<T?, Task>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = 3,
        bool dlx = false,
        bool isExclusive = false)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(queue, persistence, isExclusive);
        if (isExclusive)
        {
            Consume(
                normalExchangeParam,
                normalQueueParam,
                null,
                null,
                resolve,
                prefetchCount,
                consumeRetry);
        }
        var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, true) : null;
        var dlxQueueParam = dlx ? GetQueueParam(queue, persistence, isExclusive, true) : null;
        Consume(
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam,
            dlxQueueParam,
            resolve,
            prefetchCount,
            consumeRetry);
    }

    public void Subscribe<T>(string topic,
        Func<Action<T?>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = 3,
        bool dlx = false,
        bool isExclusive = false)
    {
        var queue = GetQueueName(resolve);
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(queue, persistence, isExclusive);
        if (isExclusive)
        {
            Consume(
                normalExchangeParam,
                normalQueueParam,
                null,
                null,
                resolve,
                prefetchCount,
                consumeRetry);
        }
        var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, true) : null;
        var dlxQueueParam = dlx ? GetQueueParam(queue, persistence, isExclusive, true) : null;
        Consume(
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam,
            dlxQueueParam,
            resolve,
            prefetchCount,
            consumeRetry);
    }

    public void Subscribe<T>(string topic,
        Func<Func<T?, Task>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = 3,
        bool dlx = false,
        bool isExclusive = false)
    {
        var queue = GetQueueName(resolve);
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(queue, persistence, isExclusive);
        if (isExclusive)
        {
            Consume(
                normalExchangeParam,
                normalQueueParam,
                null,
                null,
                resolve,
                prefetchCount,
                consumeRetry);
        }
        var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, true) : null;
        var dlxQueueParam = dlx ? GetQueueParam(queue, persistence, isExclusive, true) : null;
        Consume(
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam,
            dlxQueueParam,
            resolve,
            prefetchCount,
            consumeRetry);
    }

    public void Subscribe(string topic,
        Func<Action<byte[]>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = 3,
        bool dlx = false,
        bool isExclusive = false)
    {
        var queue = GetQueueName(resolve);
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(queue, persistence, isExclusive);
        if (isExclusive)
        {
            Consume(
                normalExchangeParam,
                normalQueueParam,
                null,
                null,
                resolve,
                prefetchCount,
                consumeRetry);
        }
        var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, true) : null;
        var dlxQueueParam = dlx ? GetQueueParam(queue, persistence, isExclusive, true) : null;
        Consume(
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam,
            dlxQueueParam,
            resolve,
            prefetchCount,
            consumeRetry);
    }

    public void Subscribe(string topic,
        Func<Func<byte[], Task>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = 3,
        bool dlx = false,
        bool isExclusive = false)
    {
        var queue = GetQueueName(resolve);
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(queue, persistence, isExclusive);
        if (isExclusive)
        {
            Consume(
                normalExchangeParam,
                normalQueueParam,
                null,
                null,
                resolve,
                prefetchCount,
                consumeRetry);
        }
        var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, true) : null;
        var dlxQueueParam = dlx ? GetQueueParam(queue, persistence, isExclusive, true) : null;
        Consume(
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam,
            dlxQueueParam,
            resolve,
            prefetchCount,
            consumeRetry);
    }
}