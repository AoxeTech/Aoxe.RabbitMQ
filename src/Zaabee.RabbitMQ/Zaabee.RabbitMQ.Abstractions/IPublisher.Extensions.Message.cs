namespace Zaabee.RabbitMQ.Abstractions;

public static partial class PublisherExtension
{
    /// <summary>
    /// Publish the message to the default topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="message"></param>
    /// <param name="publishRetry"></param>
    /// <typeparam name="T"></typeparam>
    public static void PublishMessage<T>(
        this IPublisher publisher,
        T? message,
        int publishRetry = Consts.DefaultPublishRetry) =>
        publisher.Publish(message, false, publishRetry);

    /// <summary>
    /// Publish the message to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="message"></param>
    /// <param name="publishRetry"></param>
    /// <typeparam name="T"></typeparam>
    public static void PublishMessage<T>(
        this IPublisher publisher,
        string topic,
        T? message,
        int publishRetry = Consts.DefaultPublishRetry) =>
        publisher.Publish(topic, message, false, publishRetry);

    /// <summary>
    /// Publish the message to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="publishRetry"></param>
    public static void PublishMessage(
        this IPublisher publisher,
        string topic,
        byte[] body,
        int publishRetry = Consts.DefaultPublishRetry) =>
        publisher.Publish(topic, body, false, publishRetry);
}