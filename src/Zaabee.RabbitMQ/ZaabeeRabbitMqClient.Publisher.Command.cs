namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void SendCommand<T>(
        T command,
        int retry = 3,
        bool dlx = true) =>
        SendMessage(command, true, retry, dlx);

    /// <inheritdoc />
    public void SendCommand<T>(
        string topic,
        T command,
        int retry = 3,
        bool dlx = true) =>
        SendMessage(topic, command, true, retry, dlx);

    /// <inheritdoc />
    public void SendCommand(
        string topic,
        byte[] body,
        int retry = 3,
        bool dlx = true) =>
        SendMessage(topic, body, true, retry, dlx);
}