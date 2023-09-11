namespace Zaabee.RabbitMQ.Abstractions;

public static partial class PublisherExtension
{
    /// <summary>
    /// Publish the event to the default topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="event"></param>
    /// <param name="retry"></param>
    /// <typeparam name="T"></typeparam>
    public static void PublishEvent<T>(
        this IPublisher publisher,
        T @event,
        int retry = 3) =>
        publisher.Publish(@event, true, retry);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="event"></param>
    /// <param name="retry"></param>
    /// <typeparam name="T"></typeparam>
    public static void PublishEvent<T>(
        this IPublisher publisher,
        string topic,
        T @event,
        int retry = 3) =>
        publisher.Publish(topic, @event, true, retry);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="retry"></param>
    public static void PublishEvent(
        this IPublisher publisher,
        string topic,
        byte[] body,
        int retry = 3) =>
        publisher.Publish(topic, body, true, retry);
}