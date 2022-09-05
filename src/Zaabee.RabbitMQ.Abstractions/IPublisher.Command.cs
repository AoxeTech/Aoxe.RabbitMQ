namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    /// <summary>
    /// Send the command to the default topic.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    void SendCommand<T>(T command, int retry = 3, bool dlx = true);

    /// <summary>
    /// Send the command to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="command"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    void SendCommand<T>(string topic, T command, int retry = 3, bool dlx = true);

    /// <summary>
    /// Send the command to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    void SendCommand(string topic, byte[] body, int retry = 3, bool dlx = true);
}