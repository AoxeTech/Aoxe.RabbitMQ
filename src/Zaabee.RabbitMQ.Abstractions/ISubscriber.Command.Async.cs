namespace Zaabee.RabbitMQ.Abstractions;

public partial interface ISubscriber
{
    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the default topic.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task ReceiveCommandAsync<T>(
        Func<Action<T?>> resolve,
        ushort prefetchCount = 10,
        int retry = 3,
        bool dlx = true);

    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the default topic.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task ReceiveCommandAsync<T>(
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = 10,
        int retry = 3,
        bool dlx = true);

    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task ReceiveCommandAsync<T>(
        string topic, Func<Action<T?>> resolve,
        ushort prefetchCount = 10,
        int retry = 3,
        bool dlx = true);

    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task ReceiveCommandAsync<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = 10,
        int retry = 3,
        bool dlx = true);

    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task ReceiveCommandAsync<T>(
        string topic,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = 10,
        int retry = 3,
        bool dlx = true);

    /// <summary>
    /// The subscriber cluster will get the command from the queue which bind the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task ReceiveCommandAsync<T>(
        string topic,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = 10,
        int retry = 3, bool dlx = true);
}