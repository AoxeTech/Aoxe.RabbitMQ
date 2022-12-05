namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public Task SendMessageAsync<T>(
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        SendMessage(message, persistence, retry, dlx);
        return Task.CompletedTask;
    }

    public Task SendMessageAsync<T>(
        string topic,
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        SendMessage(topic, message, persistence, retry, dlx);
        return Task.CompletedTask;
    }

    public Task SendMessageAsync(
        string topic,
        byte[] body,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        SendMessage(topic, body, persistence, retry, dlx);
        return Task.CompletedTask;
    }
}