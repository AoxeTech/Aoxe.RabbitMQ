namespace Zaabee.RabbitMQ.Abstractions;

public static partial class PublisherExtension
{
    /// <summary>
    /// Publish the event to the default topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="event"></param>
    /// <param name="retry"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ValueTask PublishEventAsync<T>(
        this IPublisher publisher,
        T @event,
        int retry = 3,
        CancellationToken cancellationToken = default) =>
        publisher.PublishAsync(@event, true, retry, cancellationToken);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="event"></param>
    /// <param name="retry"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ValueTask PublishEventAsync<T>(
        this IPublisher publisher,
        string topic,
        T @event,
        int retry = 3,
        CancellationToken cancellationToken = default) =>
        publisher.PublishAsync(topic, @event, true, retry, cancellationToken);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="retry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static ValueTask PublishEventAsync(
        this IPublisher publisher,
        string topic,
        byte[] body,
        int retry = 3,
        CancellationToken cancellationToken = default) =>
        publisher.PublishAsync(topic, body, true, retry, cancellationToken);
}