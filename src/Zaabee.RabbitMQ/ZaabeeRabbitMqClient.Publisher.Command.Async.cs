namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public Task SendCommandAsync<T>(
        T command,
        int retry = 3,
        bool dlx = true)
    {
        SendCommand(command, retry, dlx);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SendCommandAsync<T>(
        string topic,
        T command,
        int retry = 3,
        bool dlx = true)
    {
        SendCommand(topic, command, retry, dlx);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SendCommandAsync(
        string topic,
        byte[] body,
        int retry = 3,
        bool dlx = true)
    {
        SendCommand(topic, body, retry, dlx);
        return Task.CompletedTask;
    }
}