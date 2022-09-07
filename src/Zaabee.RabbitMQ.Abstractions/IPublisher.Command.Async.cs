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
    /// <returns></returns>
    Task SendCommandAsync<T>(
        T command,
        int retry = 3,
        bool dlx = true);

    /// <summary>
    /// Send the command to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="command"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SendCommandAsync<T>(
        string topic,
        T command,
        int retry = 3,
        bool dlx = true);

    /// <summary>
    /// Send the command to the specified topic.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="body"></param>
    /// <param name="retry"></param>
    /// <param name="dlx"></param>
    /// <returns></returns>
    Task SendCommandAsync(
        string topic,
        byte[] body,
        int retry = 3,
        bool dlx = true);
}