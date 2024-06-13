namespace Aoxe.RabbitMQ.Abstractions;

public static partial class SubscriberExtension
{
    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the default topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    public static void SubscribeMessage<T>(
        this ISubscriber subscriber,
        Func<Func<T?, ValueTask>> resolve,
        string? queueName = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount
    )
    {
        var topic = InternalHelper.GetTopicName(typeof(T));
        queueName = queueName is null
            ? InternalHelper.GenerateQueueName(resolve, true)
            : $"{queueName}[{Guid.NewGuid()}]";
        subscriber.Subscribe(topic, resolve, queueName, false, prefetchCount, 0, false, false);
    }

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    public static void SubscribeMessage<T>(
        this ISubscriber subscriber,
        string topic,
        Func<Func<T?, ValueTask>> resolve,
        string? queueName = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount
    )
    {
        queueName = queueName is null
            ? InternalHelper.GenerateQueueName(resolve, true)
            : $"{queueName}[{Guid.NewGuid()}]";
        subscriber.Subscribe(topic, resolve, queueName, false, prefetchCount, 0, false, false);
    }

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="prefetchCount"></param>
    public static void SubscribeMessage(
        this ISubscriber subscriber,
        string topic,
        Func<Func<byte[], ValueTask>> resolve,
        string? queueName = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount
    )
    {
        queueName = queueName is null
            ? InternalHelper.GenerateQueueName(resolve, true)
            : $"{queueName}[{Guid.NewGuid()}]";
        subscriber.Subscribe(topic, resolve, queueName, false, prefetchCount, 0, false, false);
    }
}
