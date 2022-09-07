namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public Task SendCommandAsync<T>(
        T command,
        int retry = 3,
        bool dlx = true) =>
        Task.Run(() => { SendCommand(command, retry, dlx); });

    public Task SendCommandAsync<T>(
        string topic,
        T command,
        int retry = 3,
        bool dlx = true) =>
        Task.Run(() => { SendCommand(topic, command, retry, dlx); });

    public Task SendCommandAsync(
        string topic,
        byte[] body,
        int retry = 3,
        bool dlx = true) =>
        Task.Run(() => { SendCommand(topic, body, retry, dlx); });
}