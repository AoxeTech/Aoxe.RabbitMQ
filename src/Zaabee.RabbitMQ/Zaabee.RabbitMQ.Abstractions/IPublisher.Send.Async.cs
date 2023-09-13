namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    /// <summary>
    /// Send the message to the default topic and generate the default queue.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="persistence"></param>
    /// <param name="publishRetry"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ValueTask SendAsync<T>(
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Send the message to the specified topic and generate the default queue.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="message"></param>
    /// <param name="persistence"></param>
    /// <param name="publishRetry"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ValueTask SendAsync<T>(
        string topic,
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Send the message to the specified topic and generate the default queue.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="persistence"></param>
    /// <param name="publishRetry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask SendAsync(
        string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        CancellationToken cancellationToken = default);
}