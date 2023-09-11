namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public Task PublishAsync<T>(
        T message,
        bool persistence = false)
    {
        Publish(message, persistence);
        return Task.CompletedTask;
    }

    public Task PublishAsync<T>(
        string topic,
        T message,
        bool persistence = false)
    {
        Publish(topic, message, persistence);
        return Task.CompletedTask;
    }

    public Task PublishAsync(
        string topic,
        byte[] body,
        bool persistence = false)
    {
        Publish(topic, body, persistence);
        return Task.CompletedTask;
    }
}