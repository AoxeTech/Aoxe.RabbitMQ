namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public Task PublishEventAsync<T>(T @event)
    {
        PublishEvent(@event);
        return Task.CompletedTask;
    }

    public Task PublishEventAsync<T>(string topic, T @event)
    {
        PublishEvent(topic, @event);
        return Task.CompletedTask;
    }

    public Task PublishEventAsync(string topic, byte[] body)
    {
        PublishEvent(topic, body);
        return Task.CompletedTask;
    }
}