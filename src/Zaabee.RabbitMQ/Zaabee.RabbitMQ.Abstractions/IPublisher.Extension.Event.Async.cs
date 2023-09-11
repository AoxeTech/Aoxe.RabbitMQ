namespace Zaabee.RabbitMQ.Abstractions;

public static partial class PublisherExtension
{
    /// <summary>
    /// Publish the event to the default topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="event"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ValueTask PublishEventAsync<T>(
        this IPublisher publisher,
        T @event) =>
        publisher.PublishAsync(@event, true);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="event"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ValueTask PublishEventAsync<T>(
        this IPublisher publisher,
        string topic,
        T @event) =>
        publisher.PublishAsync(topic, @event, true);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static ValueTask PublishEventAsync(
        this IPublisher publisher,
        string topic,
        byte[] body) =>
        publisher.PublishAsync(topic, body, true);
}