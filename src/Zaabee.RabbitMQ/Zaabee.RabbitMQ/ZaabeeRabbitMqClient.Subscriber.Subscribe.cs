namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Subscribe<T>(
        Func<Action<T?>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
        bool isExclusive = false) =>
        Subscribe(GetTypeName(typeof(T)),
            resolve,
            persistence,
            prefetchCount,
            consumeRetry,
            dlx,
            isExclusive);

    /// <inheritdoc />
    public void Subscribe<T>(
        Func<Func<T?, Task>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
        bool isExclusive = false) =>
        Subscribe(GetTypeName(typeof(T)),
            resolve,
            persistence,
            prefetchCount,
            consumeRetry,
            dlx,
            isExclusive);

    /// <inheritdoc />
    public void Subscribe<T>(
        string topic,
        Func<Action<T?>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
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
        else
        {
            var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, ExchangeRole.Dlx) : null;
            var dlxQueueParam = dlx ? GetQueueParam(queue, persistence, isExclusive, QueueRole.Dlx) : null;
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

    /// <inheritdoc />
    public void Subscribe<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
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
        else
        {
            var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, ExchangeRole.Dlx) : null;
            var dlxQueueParam = dlx ? GetQueueParam(queue, persistence, isExclusive, QueueRole.Dlx) : null;
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

    /// <inheritdoc />
    public void Subscribe(
        string topic,
        Func<Action<byte[]>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
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
        else
        {
            var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, ExchangeRole.Dlx) : null;
            var dlxQueueParam = dlx ? GetQueueParam(queue, persistence, isExclusive, QueueRole.Dlx) : null;
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

    /// <inheritdoc />
    public void Subscribe(
        string topic,
        Func<Func<byte[], Task>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
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
        else
        {
            var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, ExchangeRole.Dlx) : null;
            var dlxQueueParam = dlx ? GetQueueParam(queue, persistence, isExclusive, QueueRole.Dlx) : null;
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
}