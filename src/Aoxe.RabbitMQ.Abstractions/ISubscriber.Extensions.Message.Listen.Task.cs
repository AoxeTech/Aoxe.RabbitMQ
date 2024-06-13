namespace Aoxe.RabbitMQ.Abstractions;

public static partial class SubscriberExtension
{
    /// <summary>
    /// The subscriber node will get the message from its own queue which bind the default topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    public static void ListenMessage<T>(
        this ISubscriber subscriber,
        Func<Func<T?, Task>> resolve,
        string? queueName = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount
    )
    {
        var topic = InternalHelper.GetTopicName(typeof(T));
        queueName = queueName is null
            ? InternalHelper.GenerateQueueName(resolve, true)
            : $"{queueName}[{Guid.NewGuid()}]";
        subscriber.Subscribe(topic, resolve, queueName, false, prefetchCount, 0, false, true);
    }

    /// <summary>
    /// The subscriber node will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    public static void ListenMessage<T>(
        this ISubscriber subscriber,
        string topic,
        Func<Func<T?, Task>> resolve,
        string? queueName = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount
    )
    {
        queueName = queueName is null
            ? InternalHelper.GenerateQueueName(resolve, true)
            : $"{queueName}[{Guid.NewGuid()}]";
        subscriber.Subscribe(topic, resolve, queueName, false, prefetchCount, 0, false, true);
    }

    /// <summary>
    /// The subscriber node will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="queueName"></param>
    /// <param name="prefetchCount"></param>
    public static void ListenMessage(
        this ISubscriber subscriber,
        string topic,
        Func<Func<byte[], Task>> resolve,
        string? queueName = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount
    )
    {
        queueName = queueName is null
            ? InternalHelper.GenerateQueueName(resolve, true)
            : $"{queueName}[{Guid.NewGuid()}]";
        subscriber.Subscribe(topic, resolve, queueName, false, prefetchCount, 0, false, true);
    }
}
