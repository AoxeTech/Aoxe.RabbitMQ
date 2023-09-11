namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public ValueTask SendAsync<T>(
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        Send(message, persistence, retry, dlx);
        return Task.CompletedTask;
    }

    public ValueTask SendAsync<T>(
        string topic,
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        Send(topic, message, persistence, retry, dlx);
        return Task.CompletedTask;
    }

    public ValueTask SendAsync(
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