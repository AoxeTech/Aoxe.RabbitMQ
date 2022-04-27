namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task SendEventAsync<T>(T @event) =>
        await Task.Run(() => { SendEvent(@event); });

    public async Task SendEventAsync(string topic, byte[] body) =>
        await Task.Run(() => { SendEvent(topic, body); });

    public async Task PublishEventAsync<T>(T @event) =>
        await Task.Run(() => { PublishEvent(@event); });

    public async Task PublishEventAsync<T>(string topic, T @event) =>
        await Task.Run(() => { PublishEvent(topic, @event); });

    public async Task PublishEventAsync(string topic, byte[] body) =>
        await Task.Run(() => { PublishEvent(topic, body); });
}