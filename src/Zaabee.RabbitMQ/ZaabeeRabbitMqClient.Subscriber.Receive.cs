namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Receive<T>(
        Func<Action<T?>> resolve,
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
        Func<Action<T?>> resolve,
        bool persistence,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry) =>
        Consume(CreateExchangeParam(topic, persistence),
            CreateQueueParam(topic, persistence),
            null,
            null,
            resolve,
            prefetchCount,
            consumeRetry);

    /// <inheritdoc />
    public void Receive(
        string topic,
        Func<Action<byte[]>> resolve,
        bool persistence,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry) =>
        Consume(CreateExchangeParam(topic, persistence),
            CreateQueueParam(topic, persistence),
            null,
            null,
            resolve,
            prefetchCount,
            consumeRetry);
}