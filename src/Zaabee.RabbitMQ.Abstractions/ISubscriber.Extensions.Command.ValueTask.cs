namespace Zaabee.RabbitMQ.Abstractions;

public static partial class SubscriberExtension
{
    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the default topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="consumeRetry"></param>
    /// <typeparam name="T"></typeparam>
    public static void ReceiveCommand<T>(
        this ISubscriber subscriber,
        Func<Func<T?, ValueTask>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
    {
        var topic = InternalHelper.GetTopicName(typeof(T));
        subscriber.Subscribe(topic, resolve, topic, true, prefetchCount, consumeRetry);
    }

    /// <summary>t
    /// The subscriber cluster will get the command from the queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="consumeRetry"></param>
    /// <typeparam name="T"></typeparam>
    public static void ReceiveCommand<T>(
        this ISubscriber subscriber,
        string topic,
        Func<Func<T?, ValueTask>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry) =>
        subscriber.Subscribe(topic, resolve, topic, true, prefetchCount, consumeRetry);

    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="consumeRetry"></param>
    public static void ReceiveCommand(
        this ISubscriber subscriber,
        string topic,
        Func<Func<byte[], ValueTask>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry) =>
        subscriber.Subscribe(topic, resolve, topic, true, prefetchCount, consumeRetry);
}