namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task SendMessageAsync<T>(T message) =>
        await Task.Run(() => { SendMessage(message); });

    public async Task SendMessageAsync(string topic, byte[] body) =>
        await Task.Run(() => { SendMessage(topic, body); });

    public async Task PublishMessageAsync<T>(T message) =>
        await Task.Run(() => { PublishMessage(message); });

    public async Task PublishMessageAsync<T>(string topic, T message) =>
        await Task.Run(() => { PublishMessage(topic, message); });

    public async Task PublishMessageAsync(string topic, byte[] body) =>
        await Task.Run(() => { PublishMessage(topic, body); });
}