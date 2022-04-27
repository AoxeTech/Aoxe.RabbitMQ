namespace Zaabee.RabbitMQ.Abstractions;

public partial interface ISubscriber
{
    /// <summary>
    /// The subscriber cluster will receive the Message by the default queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task ReceiveMessageAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Message by the default queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task ReceiveMessageAsync<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Message by its own queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeMessageAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Message by its own queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeMessageAsync<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Message by the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeMessageAsync<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Message by the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeMessageAsync<T>(string topic, Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber node will receive the Message by its own queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task ListenMessageAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber node will receive the Message by its own queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task ListenMessageAsync<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = 10);
}