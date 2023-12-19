namespace Zaabee.RabbitMQ.Abstractions;

public interface IPublisher
{
    /// <summary>
    /// Publish the message to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="message"></param>
    /// <param name="persistence"></param>
    /// <param name="publishRetry"></param>
    /// <param name="queueName"></param>
    /// <typeparam name="T"></typeparam>
    void Publish<T>(
        string topic,
        T? message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        string? queueName = null
    );

    /// <summary>
    /// Publish the message to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="persistence"></param>
    /// <param name="publishRetry"></param>
    /// <param name="queueName"></param>
    void Publish(
        string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        string? queueName = null
    );
}
