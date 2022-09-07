namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public Task PublishEventAsync<T>(T @event) =>
        Task.Run(() => { PublishEvent(@event); });

    public Task PublishEventAsync<T>(string topic, T @event) =>
        Task.Run(() => { PublishEvent(topic, @event); });

    public Task PublishEventAsync(string topic, byte[] body) =>
        Task.Run(() => { PublishEvent(topic, body); });
}