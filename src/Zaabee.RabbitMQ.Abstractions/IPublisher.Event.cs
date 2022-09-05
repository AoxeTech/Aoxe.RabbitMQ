namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    /// <summary>
    /// Publish the event to the default topic.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    void PublishEvent<T>(T @event, int retry = 3, bool dlx = true);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="event"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    void PublishEvent<T>(string topic, T @event, int retry = 3, bool dlx = true);

    /// <summary>
    /// Publish the event to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    void PublishEvent(string topic, byte[] body, int retry = 3, bool dlx = true);
}