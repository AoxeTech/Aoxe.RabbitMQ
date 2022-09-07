namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    /// <summary>
    /// Publish the event to the default topic.
    /// </summary>
    /// <param name="event"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task PublishEventAsync<T>(
        T @event);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="event"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task PublishEventAsync<T>(
        string topic,
        T @event);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    Task PublishEventAsync(
        string topic,
        byte[] body);
}