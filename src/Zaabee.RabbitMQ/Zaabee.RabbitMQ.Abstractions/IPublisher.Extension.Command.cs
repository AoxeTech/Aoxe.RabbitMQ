namespace Zaabee.RabbitMQ.Abstractions;

public static partial class PublisherExtension
{
    /// <summary>
    /// Send the command to the default topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="command"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    public static void SendCommand<T>(
        this IPublisher publisher,
        T command,
        int retry = 3,
        bool dlx = true) =>
        publisher.Send(command, true, retry, dlx);

    /// <summary>
    /// Send the command to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="command"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    public static void SendCommand<T>(
        this IPublisher publisher,
        string topic,
        T command,
        int retry = 3,
        bool dlx = true) =>
        publisher.Send(topic, command, true, retry, dlx);

    /// <summary>
    /// Send the command to the specified topic.
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    public static void SendCommand(
        this IPublisher publisher,
        string topic,
        byte[] body,
        int retry = 3,
        bool dlx = true) =>
        publisher.Send(topic, body, true, retry, dlx);
}