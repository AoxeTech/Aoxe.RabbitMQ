namespace Zaabee.RabbitMQ.Abstractions;

public static partial class PublisherExtension
{
    /// <summary>
    /// Send the command to the default topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="command"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ValueTask SendCommandAsync<T>(
        this IPublisher publisher,
        T command,
        int retry = 3,
        bool dlx = true,
        CancellationToken cancellationToken = default) =>
        publisher.SendAsync(command, true, retry, dlx, cancellationToken);

    /// <summary>
    /// Send the command to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="command"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ValueTask SendCommandAsync<T>(
        this IPublisher publisher,
        string topic,
        T command,
        int retry = 3,
        bool dlx = true,
        CancellationToken cancellationToken = default) =>
        publisher.SendAsync(topic, command, true, retry, dlx, cancellationToken);

    /// <summary>
    /// Send the command to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static ValueTask SendCommandAsync(
        this IPublisher publisher,
        string topic,
        byte[] body,
        int retry = 3,
        bool dlx = true,
        CancellationToken cancellationToken = default) =>
        publisher.SendAsync(topic, body, true, retry, dlx, cancellationToken);
}