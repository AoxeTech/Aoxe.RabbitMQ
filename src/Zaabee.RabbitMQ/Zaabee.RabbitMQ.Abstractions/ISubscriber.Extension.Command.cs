namespace Zaabee.RabbitMQ.Abstractions;

public static partial class SubscriberExtension
{
    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the default topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    public static void ReceiveCommand<T>(
        this ISubscriber subscriber,
        Func<Action<T?>> resolve,
        ushort prefetchCount = 10,
        bool dlx = true) =>
        subscriber.Receive(resolve, true, prefetchCount, dlx);

    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the default topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    public static void ReceiveCommand<T>(
        this ISubscriber subscriber,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = 10,
        bool dlx = true) =>
        subscriber.Receive(resolve, true, prefetchCount, dlx);

    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    public static void ReceiveCommand<T>(
        this ISubscriber subscriber,
        string topic,
        Func<Action<T?>> resolve,
        ushort prefetchCount = 10,
        bool dlx = true) =>
        subscriber.Receive(topic, resolve, true, prefetchCount, dlx);

    /// <summary>t
    /// The subscriber cluster will get the command from the queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    public static void ReceiveCommand<T>(
        this ISubscriber subscriber,
        string topic,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = 10,
        bool dlx = true) =>
        subscriber.Receive(topic, resolve, true, prefetchCount, dlx);

    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    public static void ReceiveCommand(
        this ISubscriber subscriber,
        string topic,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = 10,
        bool dlx = true) =>
        subscriber.Receive(topic, resolve, true, prefetchCount, dlx);

    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the specified topic.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    public static void ReceiveCommand(
        this ISubscriber subscriber,
        string topic,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = 10,
        bool dlx = true) =>
        subscriber.Receive(topic, resolve, true, prefetchCount, dlx);
}