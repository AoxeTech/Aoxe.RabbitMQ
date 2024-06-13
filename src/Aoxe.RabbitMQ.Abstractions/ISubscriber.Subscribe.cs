namespace Aoxe.RabbitMQ.Abstractions;

public partial interface ISubscriber
{
    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="consumeRetry"></param>
    /// <param name="dlx"></param>
    /// <param name="isExclusive"></param>
    /// <typeparam name="T"></typeparam>
    void Subscribe<T>(
        string topic,
        Func<Action<T?>> resolve,
        string queueName,
        bool persistence = true,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
        bool isExclusive = false
    );

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="consumeRetry"></param>
    /// <param name="dlx"></param>
    /// <param name="isExclusive"></param>
    void Subscribe(
        string topic,
        Func<Action<byte[]>> resolve,
        string queueName,
        bool persistence = true,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
        bool isExclusive = false
    );
}
