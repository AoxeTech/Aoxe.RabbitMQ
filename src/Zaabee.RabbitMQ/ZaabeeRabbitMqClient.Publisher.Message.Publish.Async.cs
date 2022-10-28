namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public Task PublishMessageAsync<T>(
        T message,
        bool persistence = false)
    {
        PublishMessage(message, persistence);
        return Task.CompletedTask;
    }

    public Task PublishMessageAsync<T>(
        string topic,
        T message,
        bool persistence = false)
    {
        PublishMessage(topic, message, persistence);
        return Task.CompletedTask;
    }

    public Task PublishMessageAsync(
        string topic,
        byte[] body,
        bool persistence = false)
    {
        PublishMessage(topic, body, persistence);
        return Task.CompletedTask;
    }
}