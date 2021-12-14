namespace Zaabee.RabbitMQ.Abstractions;

public partial interface ISubscriber
{
    /// <summary>
    /// The subscriber cluster will receive the Command by the default queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    void ReceiveCommand<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

    /// <summary>
    /// The subscriber cluster will receive the Command by the default queue.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="prefetchCount"></param>
    /// <typeparam name="T"></typeparam>
    void ReceiveCommand<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = 10);
}