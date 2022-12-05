namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public Task SendAsync<T>(
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        Send(message, persistence, retry, dlx);
        return Task.CompletedTask;
    }

    public Task SendAsync<T>(
        string topic,
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        Send(topic, message, persistence, retry, dlx);
        return Task.CompletedTask;
    }

    public Task SendAsync(
        string topic,
        byte[] body,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        Send(topic, body, persistence, retry, dlx);
        return Task.CompletedTask;
    }
}