namespace Zaabee.RabbitMQ.Abstractions;

public partial interface ISubscriber
{
    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the default topic.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ValueTask SubscribeAsync<T>(
        Func<Action<T?>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        bool dlx = false);

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the default topic.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ValueTask SubscribeAsync<T>(
        Func<Func<T?, Task>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        bool dlx = false);

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ValueTask SubscribeAsync<T>(
        string topic,
        Func<Action<T?>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        bool dlx = false);

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ValueTask SubscribeAsync<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        bool dlx = false);

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    /// <returns></returns>
    ValueTask SubscribeAsync(
        string topic,
        Func<Action<byte[]>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        bool dlx = false);

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="dlx"></param>
    /// <returns></returns>
    ValueTask SubscribeAsync(
        string topic,
        Func<Func<byte[], Task>> resolve,
        bool persistence,
        ushort prefetchCount = 10,
        bool dlx = false);
}