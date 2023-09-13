namespace Zaabee.RabbitMQ.Abstractions;

public static partial class PublisherExtension
{
    /// <summary>
    /// Publish the Message to the default topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="message"></param>
    /// <param name="publishRetry"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ValueTask PublishMessageAsync<T>(
        this IPublisher publisher,
        T message,
        int publishRetry = Consts.DefaultPublishRetry,
        CancellationToken cancellationToken = default) =>
        publisher.PublishAsync(message, false, publishRetry, cancellationToken);

    /// <summary>
    /// Publish the Message to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="message"></param>
    /// <param name="publishRetry"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ValueTask PublishMessageAsync<T>(
        this IPublisher publisher,
        string topic,
        T message,
        int publishRetry = Consts.DefaultPublishRetry,
        CancellationToken cancellationToken = default) =>
        publisher.PublishAsync(topic, message, false, publishRetry, cancellationToken);

    /// <summary>
    /// Publish the Message to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="publishRetry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static ValueTask PublishMessageAsync(
        this IPublisher publisher,
        string topic,
        byte[] body,
        int publishRetry = Consts.DefaultPublishRetry,
        CancellationToken cancellationToken = default) =>
        publisher.PublishAsync(topic, body, false, publishRetry, cancellationToken);
}