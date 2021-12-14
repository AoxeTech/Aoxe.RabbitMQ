namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task PublishEventAsync<T>(T @event) =>
        await Task.Run(() => { PublishEvent(@event); });

    public async Task PublishEventAsync<T>(string exchangeName, T @event) =>
        await Task.Run(() => { PublishEvent(exchangeName, @event); });

    public async Task PublishEventAsync(string exchangeName, byte[] body) =>
        await Task.Run(() => { PublishEvent(exchangeName, body); });
}