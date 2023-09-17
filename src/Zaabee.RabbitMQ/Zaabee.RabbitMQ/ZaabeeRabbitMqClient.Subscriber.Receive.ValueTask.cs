namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Receive<T>(
        Func<Func<T?, ValueTask>> resolve,
        bool persistence,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry) =>
        Receive(GetTopicName(typeof(T)),
            resolve,
            persistence,
            prefetchCount,
            consumeRetry);

    /// <inheritdoc />
    public void Receive<T>(
        string topic,
        Func<Func<T?, ValueTask>> resolve,
        bool persistence,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry) =>
        Consume(GetExchangeParam(topic, persistence),
            GetQueueParam(topic, persistence),
            null,
            null,
            resolve,
            prefetchCount,
            consumeRetry);

    /// <inheritdoc />
    public void Receive(string topic,
        Func<Func<byte[], ValueTask>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        int consumeRetry = Consts.DefaultConsumeRetry) =>
        Consume(
            GetExchangeParam(topic, persistence),
            GetQueueParam(topic, persistence),
            null,
            null,
            resolve,
            prefetchCount,
            consumeRetry);
}