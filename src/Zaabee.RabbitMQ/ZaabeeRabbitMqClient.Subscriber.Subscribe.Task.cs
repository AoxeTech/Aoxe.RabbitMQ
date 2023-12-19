namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Subscribe<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        string queueName,
        bool persistence = true,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
        bool isExclusive = false
    )
    {
        // The exclusive queue do not have dlx
        Consume(
            CreateExchangeParam(topic, persistence),
            CreateQueueParam(queueName, persistence, isExclusive),
            dlx && !isExclusive ? CreateExchangeParam(topic, persistence, ExchangeRole.Dlx) : null,
            dlx && !isExclusive
                ? CreateQueueParam(queueName, persistence, isExclusive, QueueRole.Dlx)
                : null,
            resolve,
            prefetchCount,
            consumeRetry
        );
    }

    /// <inheritdoc />
    public void Subscribe(
        string topic,
        Func<Func<byte[], Task>> resolve,
        string queueName,
        bool persistence = true,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
        bool isExclusive = false
    )
    {
        // The exclusive queue do not have dlx
        Consume(
            CreateExchangeParam(topic, persistence),
            CreateQueueParam(queueName, persistence, isExclusive),
            dlx && !isExclusive ? CreateExchangeParam(topic, persistence, ExchangeRole.Dlx) : null,
            dlx && !isExclusive
                ? CreateQueueParam(queueName, persistence, isExclusive, QueueRole.Dlx)
                : null,
            resolve,
            prefetchCount,
            consumeRetry
        );
    }
}
