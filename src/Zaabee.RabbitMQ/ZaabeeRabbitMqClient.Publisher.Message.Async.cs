namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public Task SendMessageAsync<T>(T message) =>
        Task.Run(() => { SendMessage(message); });

    public Task SendMessageAsync(string topic, byte[] body) =>
        Task.Run(() => { SendMessage(topic, body); });

    public Task PublishMessageAsync<T>(T message) =>
        Task.Run(() => { PublishMessage(message); });

    public Task PublishMessageAsync<T>(string topic, T message) =>
        Task.Run(() => { PublishMessage(topic, message); });

    public Task PublishMessageAsync(string topic, byte[] body) =>
        Task.Run(() => { PublishMessage(topic, body); });
}