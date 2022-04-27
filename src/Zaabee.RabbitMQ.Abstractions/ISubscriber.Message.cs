namespace Zaabee.RabbitMQ.Abstractions;

public partial interface ISubscriber
{
    /// <summary>
    /// The subscriber cluster will receive the Message by the default queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    void ReceiveMessage<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Message by the default queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    void ReceiveMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Message by its own queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    void SubscribeMessage<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Message by its own queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    void SubscribeMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Message by the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    void SubscribeMessage<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Message by the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    void SubscribeMessage<T>(string topic, Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber node will receive the Message by its own queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    void ListenMessage<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber node will receive the Message by its own queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    void ListenMessage<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);
}