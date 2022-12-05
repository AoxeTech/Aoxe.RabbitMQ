namespace Zaabee.RabbitMQ.Abstractions;

public static partial class PublisherExtension
{
    /// <summary>
    /// Publish the event to the default topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="event"></param>
    /// <typeparam name="T"></typeparam>
    public static void PublishEvent<T>(
        this IPublisher publisher,
        T @event) =>
        publisher.Publish(@event, true);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="event"></param>
    /// <typeparam name="T"></typeparam>
    public static void PublishEvent<T>(
        this IPublisher publisher,
        string topic,
        T @event) =>
        publisher.Publish(topic, @event, true);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    public static void PublishEvent<T>(
        this IPublisher publisher,
        string topic,
        byte[] body) =>
        publisher.Publish(topic, body, true);
}