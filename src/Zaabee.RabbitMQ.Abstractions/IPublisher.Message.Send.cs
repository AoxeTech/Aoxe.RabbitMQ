namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    /// <summary>
    /// Send the message to the default topic.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="persistence"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    void SendMessage<T>(T message, bool persistence = false, int retry = 0, bool dlx = false);

    /// <summary>
    /// Send the message to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="message"></param>
    /// <param name="persistence"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    void SendMessage<T>(string topic, T message, bool persistence = false, int retry = 0, bool dlx = false);

    /// <summary>
    /// Send the message to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="persistence"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    void SendMessage(string topic, byte[] body, bool persistence = false, int retry = 0, bool dlx = false);
}