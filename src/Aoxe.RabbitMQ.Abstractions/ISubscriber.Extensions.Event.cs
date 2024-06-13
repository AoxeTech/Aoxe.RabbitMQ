namespace Aoxe.RabbitMQ.Abstractions;

public static partial class SubscriberExtension
{
    /// <summary>
    /// The subscriber cluster will get the event from its own queue which bind the default topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="consumeRetry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    public static void SubscribeEvent<T>(
        this ISubscriber subscriber,
        Func<Action<T?>> resolve,
        string? queueName = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true
    )
    {
        var topic = InternalHelper.GetTopicName(typeof(T));
        queueName ??= InternalHelper.GenerateQueueName(resolve, false);
        subscriber.Subscribe(
            topic,
            resolve,
            queueName,
            true,
            prefetchCount,
            consumeRetry,
            dlx,
            isExclusive: false
        );
    }

    /// <summary>
    /// The subscriber cluster will get the event from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="consumeRetry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    public static void SubscribeEvent<T>(
        this ISubscriber subscriber,
        string topic,
        Func<Action<T?>> resolve,
        string? queueName = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true
    )
    {
        queueName ??= InternalHelper.GenerateQueueName(resolve, false);
        subscriber.Subscribe(
            topic,
            resolve,
            queueName,
            true,
            prefetchCount,
            consumeRetry,
            dlx,
            isExclusive: false
        );
    }

    /// <summary>
    /// The subscriber cluster will get the event from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="consumeRetry"></param>
    /// <param name="dlx"></param>
    public static void SubscribeEvent(
        this ISubscriber subscriber,
        string topic,
        Func<Action<byte[]>> resolve,
        string? queueName = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true
    )
    {
        queueName ??= InternalHelper.GenerateQueueName(resolve, false);
        subscriber.Subscribe(
            topic,
            resolve,
            queueName,
            true,
            prefetchCount,
            consumeRetry,
            dlx,
            isExclusive: false
        );
    }
}
