namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    /// <summary>
    /// Publish the event to the default topic.
    /// </summary>
    /// <param name="event"></param>
    /// <typeparam name="T"></typeparam>
    void PublishEvent<T>(
        T @event);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="event"></param>
    /// <typeparam name="T"></typeparam>
    void PublishEvent<T>(
        string topic,
        T @event);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    void PublishEvent(
        string topic,
        byte[] body);
}