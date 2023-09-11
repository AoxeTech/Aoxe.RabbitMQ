namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public ValueTask PublishAsync<T>(
        T message,
        bool persistence = false)
    {
        Publish(message, persistence);
        return ValueTask.CompletedTask;
    }

    public ValueTask PublishAsync<T>(
        string topic,
        T message,
        bool persistence = false)
    {
        Publish(topic, message, persistence);
        return Task.CompletedTask;
    }

    public ValueTask PublishAsync(
        string topic,
        byte[] body,
        bool persistence = false)
    {
        Publish(topic, body, persistence);
        return Task.CompletedTask;
    }
}