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
    public static Task PublishEventAsync<T>(
        IPublisher publisher,
        T @event)
    {
        publisher.PublishEvent(@event);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="event"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task PublishEventAsync<T>(
        IPublisher publisher,
        string topic,
        T @event)
    {
        publisher.PublishEvent(topic, @event);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static Task PublishEventAsync<T>(
        IPublisher publisher,
        string topic,
        byte[] body)
    {
        publisher.PublishEvent(topic, body);
        return Task.CompletedTask;
    }
}