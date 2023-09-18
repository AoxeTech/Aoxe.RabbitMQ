namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Subscribe<T>(
        Func<Func<T?, Task>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
        bool isExclusive = false) =>
        Subscribe(GetTopicName(typeof(T)),
            resolve,
            persistence,
            prefetchCount,
            consumeRetry,
            dlx,
            isExclusive);

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
        var queue = GenerateQueueName(resolve);
        // The exclusive queue do not have dlx
        Consume(CreateExchangeParam(topic, persistence),
            CreateQueueParam(queue, persistence, isExclusive),
            dlx && !isExclusive ? CreateExchangeParam(topic, persistence, ExchangeRole.Dlx) : null,
            dlx && !isExclusive ? CreateQueueParam(queue, persistence, isExclusive, QueueRole.Dlx) : null,
            resolve,
            prefetchCount,
            consumeRetry);
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
        var queue = GenerateQueueName(resolve);
        // The exclusive queue do not have dlx
        Consume(CreateExchangeParam(topic, persistence),
            CreateQueueParam(queue, persistence, isExclusive),
            dlx && !isExclusive ? CreateExchangeParam(topic, persistence, ExchangeRole.Dlx) : null,
            dlx && !isExclusive ? CreateQueueParam(queue, persistence, isExclusive, QueueRole.Dlx) : null,
            resolve,
            prefetchCount,
            consumeRetry);
    }
}