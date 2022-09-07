namespace Zaabee.RabbitMQ.Abstractions;

public partial interface ISubscriber
{
    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the default topic.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeMessageAsync<T>(
        Func<Action<T?>> resolve,
        bool persistence = false,
        ushort prefetchCount = 10,
        int retry = 0,
        bool dlx = false);

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the default topic.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeMessageAsync<T>(
        Func<Func<T?, Task>> resolve,
        bool persistence = false,
        ushort prefetchCount = 10,
        int retry = 0,
        bool dlx = false);

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeMessageAsync<T>(
        string topic,
        Func<Action<T?>> resolve,
        bool persistence = false,
        ushort prefetchCount = 10,
        int retry = 0,
        bool dlx = false);

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeMessageAsync<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        bool persistence = false,
        ushort prefetchCount = 10,
        int retry = 0,
        bool dlx = false);

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeMessageAsync<T>(
        string topic,
        Func<Action<byte[]>> resolve,
        bool persistence = false,
        ushort prefetchCount = 10,
        int retry = 0,
        bool dlx = false);

    /// <summary>
    /// The subscriber cluster will get the message from its own queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="persistence"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeMessageAsync<T>(
        string topic,
        Func<Func<byte[], Task>> resolve,
        bool persistence = false,
        ushort prefetchCount = 10,
        int retry = 0,
        bool dlx = false);
}