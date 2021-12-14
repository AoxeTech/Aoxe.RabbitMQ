namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task PublishMessageAsync<T>(T message) =>
        await Task.Run(() => { PublishMessage(message); });

    public async Task PublishMessageAsync<T>(string exchangeName, T message) =>
        await Task.Run(() => { PublishMessage(exchangeName, message); });

    public async Task PublishMessageAsync(string exchangeName, byte[] body) =>
        await Task.Run(() => { PublishMessage(exchangeName, body); });
}